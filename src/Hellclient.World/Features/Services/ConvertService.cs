using Hellclient.World.States;
using Hellclient.World.Types;
using Hellclient.World.Utils;

namespace Hellclient.World.Features.Services;

public interface IConvertService
{
    //转换服务，byte与line/Command的转换和处理
    public void DoSend(WorldContext context, Command cmd);
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

public class ConvertService : IConvertService
{
    private readonly IScriptService ScriptService= new ScriptService();
    public void DoSend(WorldContext context, Command cmd)
    {
        if (cmd.Message == "\x0f")
        {
            return;
        }
        if (ScriptService.HandleSend(context, cmd.Message))
        {
            return;
        }
        var bytes = CharsetUtil.FromUtf8(context.Config.Data.Charset, cmd.Message);
        if (cmd.Echo)
        {
            DoPrintEcho(context, cmd);
        }
        if (cmd.History)
        {
            context.Info.History.Add(cmd.Message);
        }
        context.Connection.Send(bytes);
        context.Connection.Send(new byte[] { 13 });
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
        context.EventBus.LineEvent?.Invoke(this, line);
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
        context.EventBus.LineEvent?.Invoke(this, line);
    }
}