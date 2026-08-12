using System.Text;
using Hellclient.World.Configs;
using Hellclient.World.Infras.Components;
using Hellclient.World.States;
using Hellclient.World.Types;
using Hellclient.World.Utils;

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
    public Task DoSend(WorldContext context, Command cmd);
    public void DoPrint(WorldContext context, string msg);
    public void DoPrintSystem(WorldContext context, string msg);
    public void DoPrintLocalBroadcastIn(WorldContext context, string msg);
    public void DoPrintGlobalBroadcastIn(WorldContext context, string msg);
    public void DoPrintLocalBroadcastOut(WorldContext context, string msg);
    public void DoPrintGlobalBroadcastOut(WorldContext context, string msg);
    public void DoPrintSubneg(WorldContext context, string msg);
    public void DoPrintRequest(WorldContext context, string msg);
    public void DoPrintResponse(WorldContext context, string msg);

}

// 连接服务，用于处理Mud当前连接的状态和数据传输，Prompt维护
public class ConnService : IConnService
{
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
    private void Listen(WorldContext context)
    {
        context.Connection.OnDataReceived += (sender, data) =>
        {
            context.Convert.OnByte(this, data);
        };
        context.Connection.OnCommandReceived += (sender, cmd) =>
        {
            OnCommandReceived(context, cmd);
        };
        context.Convert.OnLine += (sender, line) =>
        {
            context.EventBus.LineEvent?.Invoke(this, line);
        };
    }
    public async void OnCommandReceived(WorldContext context, TelnetCommand cmd)
    {

        context.Convert.Publish();
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
        context.Convert.Publish();
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
    public async Task DoSend(WorldContext context, Command cmd)
    {
        if (cmd.Message == "\x0f")
        {
            return;
        }
        var bytes = CharsetUtil.FromUtf8(context.Config.Data.Charset, cmd.Message);
        context.Convert.Publish();
        if (cmd.Echo)
        {
            DoPrintEcho(context, cmd);
        }
        if (cmd.History)
        {
            context.Info.History.Add(cmd.Message);
        }
        await context.Connection.Send(bytes);
        await context.Connection.Send(new byte[] { 13 });
    }

    public void DoPrintEcho(WorldContext context, Command cmd)
    {
        var line = Line.New();
        line.Creator = cmd.Creator;
        line.CreatorType = cmd.CreatorType;
        line.Type = Line.LineTypeEcho;
        var w = new Word()
        {
            Text = cmd.Message,
        };
        line.Words.Add(w);
        context.EventBus.LineEvent!.Invoke(this, line);
    }
    public void DoPrintRequest(WorldContext context, string msg)
    {
        print(context, Line.LineTypeRequest, msg);
    }
    public void DoPrintResponse(WorldContext context, string msg)
    {
        print(context, Line.LineTypeResponse, msg);
    }
    public void DoPrintLocalBroadcastIn(WorldContext context, string msg)
    {
        print(context, Line.LineTypeLocalBroadcastIn, msg);
    }
    public void DoPrintGlobalBroadcastIn(WorldContext context, string msg)
    {
        print(context, Line.LineTypeGlobalBroadcastIn, msg);
    }
    public void DoPrintLocalBroadcastOut(WorldContext context, string msg)
    {
        print(context, Line.LineTypeLocalBroadcastOut, msg);
    }
    public void DoPrintGlobalBroadcastOut(WorldContext context, string msg)
    {
        print(context, Line.LineTypeGlobalBroadcastOut, msg);
    }
    public void DoPrintSubneg(WorldContext context, string msg)
    {
        print(context, Line.LineTypeSubneg, msg);
    }

    public void DoPrintSystem(WorldContext context, string msg)
    {
        print(context, Line.LineTypeSystem, msg);
    }

    public void DoPrint(WorldContext context, string msg)
    {
        print(context, Line.LineTypePrint, msg);
    }
    private void print(WorldContext context, int linetype, string msg)
    {
        var line = Line.New();
        line.Type = linetype;
        line.Words = [new() { Text = msg }];
        context.EventBus.LineEvent!.Invoke(this, line);
    }
}