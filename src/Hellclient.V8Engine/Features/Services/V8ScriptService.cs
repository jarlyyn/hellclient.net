using Hellclient.V8Engine.Features.States;

namespace Hellclient.V8Engine.Features.Services;


public interface IV8ScriptService
{
    public void InstallTo(V8EngineContext context);
    public void Open(V8EngineContext context);
    public void Run(V8EngineContext context, string script);
}
public partial class V8ScriptService : IV8ScriptService
{
    public void InstallTo(V8EngineContext context)
    {
        initEval(context);
        initMetronome(context);
        initJsAPI(context);
    }
    public void handleError(V8EngineContext context, Exception ex)
    {
        if (ex is Microsoft.ClearScript.ScriptEngineException scriptEx)
        {
            Console.WriteLine($"Script Error: {scriptEx.ErrorDetails}");
        }
        context.World.HandleScriptError(ex);
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
        context.Runtime.Execute(entry, entrydata);
        if (data.OnOpen != "")
        {
            callByName(context, data.OnOpen);
        }
    }
    private object? callByName(V8EngineContext context, string funcname)
    {
        try
        {
            var func = context.Runtime.Evaluate(funcname);
            if (func is not null && func is Microsoft.ClearScript.ScriptObject scriptfunc)
            {
                return scriptfunc.InvokeAsFunction();
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
}