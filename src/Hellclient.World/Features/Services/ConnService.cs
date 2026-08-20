using System.Text;
using Hellclient.World.Configs;
using Hellclient.World.Infras.Components;
using Hellclient.World.States;
using Hellclient.World.Types;

namespace Hellclient.World.Features.Services;

public interface IConnService
{
    public void InstallTo(WorldContext context);
    public Task Connect(WorldContext context);
    public void Stop(WorldContext context);
    public Task Close(WorldContext context);
    public void Send(WorldContext context, byte[] message);
    public bool IsConnected(WorldContext context);
    public byte[] GetBuffer(WorldContext context);

}

// 连接服务，用于处理Mud当前连接的状态和数据传输，Prompt维护
public class ConnService : IConnService
{
    public IScriptService ScriptService { get; set; } = new ScriptService();

    public static TimeSpan DefaultDebounceDuration = TimeSpan.FromMilliseconds(200);
    public void InstallTo(WorldContext context)
    {
        context.Connection.OnConnected += (sender, args) =>
        {
            context.EventBus.ConnectedEvent?.Invoke(this, EventArgs.Empty);
        };
        context.Connection.OnDisconnected += (sender, args) =>
        {
            context.EventBus.DisconnectedEvent?.Invoke(this, EventArgs.Empty);
        };
        context.Convert.OnPrompt += (sender, line) =>
        {
            context.EventBus.PromptEvent?.Invoke(this, line);
        };
        context.Convert.Debounce = new Debounce(DefaultDebounceDuration, () =>
        {
            if (context.Connection.IsConnected())
            {
                context.Convert.Prompt();
            }
        });
        Listen(context);
    }
    private void OnByte(WorldContext context, byte data)
    {
        context.Lock.Wait();
        try
        {

            if (data == 13 || data == 10)
            {

                context.Convert.Publish(true);
                return;
            }
            context.Convert.AppendBuffer(data);

            if (ScriptService.HandleBuffer(context, context.Convert.GetBuffer()))
            {
                context.Convert.Publish(true);
                return;
            }
        }
        finally
        {
            context.Lock.Release();
        }
        Task.Run(() => context.Convert.Debounce!.Exec());
    }

    private void Listen(WorldContext context)
    {
        var ctx = context;
        ctx.Connection.OnDataReceived += (sender, data) => OnByte(ctx, data);
        ctx.Connection.OnCommandReceived += (sender, cmd) => OnCommandReceived(ctx, cmd);
        ctx.Convert.OnLine += (sender, line) => ctx.EventBus.LineEvent?.Invoke(this, line);
    }
    public async void OnCommandReceived(WorldContext context, TelnetCommand cmd)
    {

        context.Convert.Publish(true);
        switch (cmd.Command)
        {
            case TelnetCommand.CmdDo:
                switch (cmd.Data[0])
                {
                    case TelnetCommand.OptionTerminalType:
                        await context.Connection.SendTelnetCommand(TelnetCommand.Will(TelnetCommand.OptionTerminalType));
                        break;
                    default:
                        break;
                }
                break;
            case TelnetCommand.CmdDont:
                switch (cmd.Data[0])
                {
                    default:
                        break;
                }
                break;
            case TelnetCommand.CmdWill:
                switch (cmd.Data[0])
                {
                    case TelnetCommand.OptionEcho:
                        await context.Connection.SendTelnetCommand(TelnetCommand.Do(TelnetCommand.OptionEcho));
                        break;
                    case TelnetCommand.OptionGMCP:
                        await context.Connection.SendTelnetCommand(TelnetCommand.Do(TelnetCommand.OptionGMCP));
                        break;
                    default:
                        break;
                }
                break;
            case TelnetCommand.CmdWont:
                switch (cmd.Data[0])
                {
                    default:
                        break;
                }
                break;
            case TelnetCommand.CmdGoAhead:
                break;
            case TelnetCommand.CmdSubnegotiation:
                if (cmd.Data.Length > 0)
                {
                    switch (cmd.Data[0])
                    {
                        case 24:
                            if (cmd.Data.Length > 1 && cmd.Data[1] == 1 && context.TType.Count > 0)
                            {
                                await context.Connection.Send(TelnetCommand.Subnegotiation(24, new byte[] { 0 }.Concat(Encoding.UTF8.GetBytes(context.TType[0])).ToArray()));
                                context.TType.RemoveAt(0);
                            }
                            break;
                        default:
                            break;
                    }
                }
                break;
        }
        context.EventBus.OnCommand?.Invoke(this, cmd);
    }

    public async Task Connect(WorldContext context)
    {
        context.Convert.Charset = context.Config.Data.Charset;
        if (AppConfig.System.TerminalType != "")
        {
            context.TType = [
                AppConfig.System.TerminalType,
                "VT100",
                "MTTS 7",
                "MTTS 7"
            ];
        }
        else
        {
            context.TType = [];
        }
        await context.Connection.Connect(context.Config.Data.Host, int.TryParse(context.Config.Data.Port, out int port) ? port : 0);
    }
    public void Stop(WorldContext context)
    {
        context.Convert.Debounce?.Discard();
    }
    public async Task Close(WorldContext context)
    {

        await context.Connection.Disconnect();
        context.Convert.Prompt();
        context.Convert.Debounce?.Discard();
        context.EventBus.ServerCloseEvent?.Invoke(this, EventArgs.Empty);
    }
    public void Send(WorldContext context, byte[] message)
    {
        context.Convert.Publish(false);
        context.Connection.Send(message);
    }
    public bool IsConnected(WorldContext context)
    {
        return context.Connection.IsConnected();
    }
    public byte[] GetBuffer(WorldContext context)
    {
        return context.Convert.GetBuffer();
    }
}