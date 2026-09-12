using System.Text;
using Hellclient.World.Configs;
using Hellclient.World.Infras.Components;
using Hellclient.World.States;
using Hellclient.World.Types;

namespace Hellclient.World.Features.Services;

public interface IConnService
{
    //原始连接相关的功能，处理byte/byte[]数据流。
    public void InstallTo(WorldContext context);
    public void Connect(WorldContext context);
    public void Stop(WorldContext context);
    public void Close(WorldContext context);
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
        Listen(context);
    }
    private void OnByte(WorldContext context, byte data)
    {

        if (data == 13 || data == 10)
        {
            context.Lock.Wait();
            try
            {

                context.Convert.Publish();
                return;
            }
            finally
            {
                context.Lock.Release();
            }
        }
        context.Convert.AppendBuffer(data);
    }

    private void Listen(WorldContext context)
    {
        var ctx = context;
        ctx.Connection.OnDataReceived += (sender, data) => OnByte(ctx, data);
        ctx.Connection.OnCommandReceived += (sender, cmd) => OnCommandReceived(ctx, cmd);
        ctx.Convert.OnLine += (sender, line) => ctx.EventBus.LineEvent?.Invoke(this, line);
    }
    public void OnCommandReceived(WorldContext context, TelnetCommand cmd)
    {
        context.Lock.Wait();
        try
        {
            switch (cmd.Command)
            {
                case TelnetCommand.CmdDo:
                    switch (cmd.Data[0])
                    {
                        case TelnetCommand.OptionTerminalType:
                            context.Connection.SendTelnetCommand(TelnetCommand.Will(TelnetCommand.OptionTerminalType));
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
                            context.Connection.SendTelnetCommand(TelnetCommand.Do(TelnetCommand.OptionEcho));
                            break;
                        case TelnetCommand.OptionGMCP:
                            context.Connection.SendTelnetCommand(TelnetCommand.Do(TelnetCommand.OptionGMCP));
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
                case TelnetCommand.CmdEndOfRecord:
                    context.Convert.Publish();
                    return;
                case TelnetCommand.CmdSubnegotiation:
                    if (cmd.Data.Length > 0)
                    {
                        switch (cmd.Data[0])
                        {
                            case 24:
                                if (cmd.Data.Length > 1 && cmd.Data[1] == 1 && context.TType.Count > 0)
                                {
                                    context.Connection.Send(TelnetCommand.Subnegotiation(24, new byte[] { 0 }.Concat(Encoding.UTF8.GetBytes(context.TType[0])).ToArray()));
                                    context.TType.RemoveAt(0);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
            }
            context.Convert.Publish();
            context.EventBus.OnCommand?.Invoke(this, cmd);
        }
        finally
        {
            context.Lock.Release();
        }
    }

    public void Connect(WorldContext context)
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
        var proxy = context.Config.Data.Proxy.Trim();
        var proxytype = "";
        var proxyhost = "";
        var proxyport = 0;
        var proxyusername = "";
        var proxypassword = "";
        if (proxy != "")
        {
            var u = Uri.TryCreate(proxy, UriKind.Absolute, out var uri) ? uri : null;
            if (u != null)
            {
                proxytype = u.Scheme;
                proxyhost = u.Host;
                proxyport = u.Port;
                if (u.UserInfo != null)
                {
                    var ui = u.UserInfo.Split(':');
                    if (ui.Length > 0) proxyusername = ui[0];
                    if (ui.Length > 1) proxypassword = ui[1];
                }

            }
        }
        context.Connection.Connect(context.Config.Data.Host, int.TryParse(context.Config.Data.Port, out int port) ? port : 0, proxytype, proxyhost, proxyport, proxyusername, proxypassword);
        context.Convert.Reset();
        context.Connection.SendTelnetCommand(TelnetCommand.Wont(TelnetCommand.OptionSGA));
        context.Connection.SendTelnetCommand(TelnetCommand.Will(TelnetCommand.OptionEOR));
    }
    public void Stop(WorldContext context)
    {
    }
    public void Close(WorldContext context)
    {

        context.Connection.Disconnect();
        context.Convert.SendPrompt();
        context.EventBus.ServerCloseEvent?.Invoke(this, EventArgs.Empty);
    }
    public void Send(WorldContext context, byte[] message)
    {
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