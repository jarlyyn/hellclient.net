using System.Dynamic;
using System.Text;
using Hellclient.V8Engine.Features.States;
using Hellclient.World.Types;

namespace Hellclient.V8Engine.Features.Services;


public interface IV8ScriptService
{
    public void InstallTo(V8EngineContext context);
    public void Open(V8EngineContext context);
    public void Run(V8EngineContext context, string script);
    public void Close(V8EngineContext context);
    public void OnConnect(V8EngineContext context);
    public void OnDisconnect(V8EngineContext context);
    public void OnTrigger(V8EngineContext context, Line line, Trigger trigger, MatchResult matchResult);
    public void OnAlias(V8EngineContext context, string message, Alias alias, MatchResult matchResult);
    public void OnTimer(V8EngineContext context, World.Types.Timer timer);
    public void OnCallback(V8EngineContext context, Callback cb);
    public void OnBroadCast(V8EngineContext context, Broadcast bc);
    public void OnHUDClick(V8EngineContext context, Click c);
    public void OnResponse(V8EngineContext context, Message msg);
    public void OnAssist(V8EngineContext context, string script);
    public bool OnBuffer(V8EngineContext context, byte[] data);
    public void OnFocus(V8EngineContext context);
    public void OnLoseFocus(V8EngineContext context);
    public void OnKeyUp(V8EngineContext context, string key);
    public bool OnSubneg(V8EngineContext context, byte code, byte[] data);
}
public partial class V8ScriptService : IV8ScriptService
{
    public void InstallTo(V8EngineContext context)
    {
        Task.Run(async () =>
        {
            try
            {
                while (true)
                {
                    await context.Timer.WaitForNextTickAsync();
                    context.Runtime.CollectGarbage(true);
                }

            }
            catch (Exception)
            {
            }

        });
        initEval(context);
        initMetronome(context);
        initJsAPI(context);
        initUserinput(context);

    }
    public void handleError(V8EngineContext context, Exception ex)
    {
        context.World.HandleScriptError(ex);
        if (ex is Microsoft.ClearScript.ScriptEngineException scriptEx)
        {
            context.World.DoPrintSystem($"[Script Error] {scriptEx.ErrorDetails}");
        }
    }
    public void Open(V8EngineContext context)
    {
        var data = context.World.GetScriptData()!;
        context.Events.OnOpen = data.OnOpen;
        context.Events.OnClose = data.OnClose;
        context.Events.OnConnect = data.OnConnect;
        context.Events.OnDisconnect = data.OnDisconnect;
        context.Events.OnBroadcast = data.OnBroadcast;
        context.Events.OnResponse = data.OnResponse;
        context.Events.OnHUDClick = data.OnHUDClick;
        context.Events.OnBuffer = data.OnBuffer;
        context.Events.OnSubneg = data.OnSubneg;
        context.Events.OnBufferMax = data.OnBufferMax;
        context.Events.OnBufferMin = data.OnBufferMin;
        context.Events.OnFocus = data.OnFocus;
        context.Events.OnLoseFocus = data.OnLoseFocus;
        context.Events.OnKeyUp = data.OnKeyUp;
        var entry = Path.Combine(context.World.GetPluginOptions().Location, "main.js");
        var entrydata = File.ReadAllText(entry);
        try
        {
            context.Runtime.Execute(entry, entrydata);
            if (data.OnOpen != "")
            {
                callByName(context, data.OnOpen);
            }
        }
        catch (Exception ex)
        {
            handleError(context, ex);
        }
    }
    private object? callByName(V8EngineContext context, string funcname, params object?[] args)
    {
        try
        {
            var func = context.Runtime.Evaluate(funcname);
            if (func is not null && func is Microsoft.ClearScript.ScriptObject scriptfunc)
            {
                return scriptfunc.InvokeAsFunction(args);
            }

        }
        catch (Exception ex)
        {
            handleError(context, ex);
        }
        return null;
    }
    public void Run(V8EngineContext context, string script)
    {
        try
        {
            context.Runtime.Execute(script);
        }
        catch (Exception ex)
        {
            handleError(context, ex);
        }
    }
    public void Close(V8EngineContext context)
    {
        if (context.Events.OnClose != "")
        {
            callByName(context, context.Events.OnClose);
        }
        context.Runtime.Dispose();
        context.Timer.Dispose();
    }
    public void OnConnect(V8EngineContext context)
    {
        if (context.Events.OnConnect != "")
        {
            callByName(context, context.Events.OnConnect);
        }
    }
    public void OnDisconnect(V8EngineContext context)
    {
        if (context.Events.OnDisconnect != "")
        {
            callByName(context, context.Events.OnDisconnect);
        }
    }
    public void OnTrigger(V8EngineContext context, Line line, Trigger trigger, MatchResult matchResult)
    {
        if (trigger.Script == "")
        {
            return;
        }
        using var model = context.Runtime.Evaluate("({})") as Microsoft.ClearScript.ScriptObject;
        if (model == null)
        {
            return;
        }
        foreach (var kv in matchResult.Named)
        {
            model[kv.Key] = kv.Value;
        }
        for (var k = 0; k < matchResult.List.Count; k++)
        {
            switch (k)
            {
                case 0:
                    model["10"] = matchResult.List[k];
                    break;
                case > 9:
                    break;

            }
            model[$"{(k - 1).ToString()}"] = matchResult.List[k];
        }
        callByName(context, trigger.Script, trigger.Name, line.ToPlainText(), model);
    }
    public void OnAlias(V8EngineContext context, string message, Alias alias, MatchResult matchResult)
    {
        if (alias.Script == "")
        {
            return;
        }
        using var model = context.Runtime.Evaluate("({})") as Microsoft.ClearScript.ScriptObject;
        if (model == null)
        {
            return;
        }

        foreach (var kv in matchResult.Named)
        {
            model[kv.Key] = kv.Value;
        }
        for (var k = 0; k < matchResult.List.Count; k++)
        {
            switch (k)
            {
                case 0:
                    model["10"] = matchResult.List[k];
                    break;
                case > 9:
                    break;

            }
            model[$"{(k - 1).ToString()}"] = matchResult.List[k];
        }
        callByName(context, alias.Script, alias.Name, message, model);

    }
    public void OnTimer(V8EngineContext context, World.Types.Timer timer)
    {
        callByName(context, timer.Script, timer.Name);
    }
    public void OnCallback(V8EngineContext context, Callback cb)
    {
        callByName(context, cb.Script, cb.Name, cb.ID, cb.Code, cb.Data);
    }
    public void OnBroadCast(V8EngineContext context, Broadcast bc)
    {
        callByName(context, context.Events.OnBroadcast, bc.Message, bc.Global, bc.Channel, bc.ID);
    }
    public void OnHUDClick(V8EngineContext context, Click c)
    {
        callByName(context, context.Events.OnHUDClick, c.X, c.Y);
    }
    public void OnResponse(V8EngineContext context, Message msg)
    {
        callByName(context, context.Events.OnResponse, msg.Type, msg.ID, msg.Data);
    }
    public void OnAssist(V8EngineContext context, string script)
    {
        callByName(context, script);
    }
    public bool OnBuffer(V8EngineContext context, byte[] data)
    {
        if (context.Events.OnBuffer == "")
        {
            return false;
        }
        var l = data.Length;
        if (l < context.Events.OnBufferMin || l > context.Events.OnBufferMax)
        {
            return false;
        }
        if (data != null)
        {
            return callByName(context, context.Events.OnBuffer, Encoding.UTF8.GetString(data), data) is bool result && result;

        }
        else
        {
            return callByName(context, context.Events.OnBuffer, null, null) is bool result && result;
        }
    }
    public void OnFocus(V8EngineContext context)
    {
        if (context.Events.OnFocus != "")
        {
            callByName(context, context.Events.OnFocus);
        }
    }
    public void OnLoseFocus(V8EngineContext context)
    {
        if (context.Events.OnLoseFocus != "")
        {
            callByName(context, context.Events.OnLoseFocus);
        }
    }
    public void OnKeyUp(V8EngineContext context, string key)
    {
        if (context.Events.OnKeyUp != "")
        {
            callByName(context, context.Events.OnKeyUp, key);
        }
    }
    public bool OnSubneg(V8EngineContext context, byte code, byte[] data)
    {
        if (context.Events.OnSubneg == "")
        {
            return false;
        }
        return callByName(context, context.Events.OnSubneg, code, data) is bool result && result;
    }

}