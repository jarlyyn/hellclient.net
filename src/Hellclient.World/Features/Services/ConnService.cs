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
            context.Convert.OnCommandReceived(this, cmd);
        };
        context.Convert.OnLine += (sender, line) =>
        {
            context.EventBus.LineEvent?.Invoke(this, line);
        };

    }
    public async Task Connect(WorldContext context)
    {
        context.Convert.Charset = context.Data.Charset;
        await context.Connection.Connect(context.Data.Host, int.TryParse(context.Data.Port, out int port) ? port : 0);
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
        context.EventBus.DisconnectedEvent?.Invoke(this, EventArgs.Empty);
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
    public async Task DoSend(WorldContext context, Command cmd)
    {
        if (cmd.Message == "\x0f")
        {
            return;
        }
        var bytes = CharsetUtil.FromUtf8(context.Data.Charset, cmd.Message);
        if (cmd.Echo)
        {
            DoPrintEcho(context, cmd);
        }
        if (cmd.History)
        {

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