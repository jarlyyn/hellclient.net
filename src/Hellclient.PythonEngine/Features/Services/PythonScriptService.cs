
using Hellclient.PythonEngine.Features.States;
using Hellclient.World.Types;
using Python.Runtime;
namespace Hellclient.PythonEngine.Features.Services;


public interface IPythonScriptService
{
    public void InstallTo(PythonEngineContext context);
    public void Open(PythonEngineContext context);
    public void Run(PythonEngineContext context, string script);
    public void Close(PythonEngineContext context);
    public void OnConnect(PythonEngineContext context);
    public void OnDisconnect(PythonEngineContext context);
    public void OnTrigger(PythonEngineContext context, Line line, Trigger trigger, MatchResult matchResult);
    public void OnAlias(PythonEngineContext context, string message, Alias alias, MatchResult matchResult);
    public void OnTimer(PythonEngineContext context, World.Types.Timer timer);
    public void OnCallback(PythonEngineContext context, Callback cb);
    public void OnBroadCast(PythonEngineContext context, Broadcast bc);
    public void OnHUDClick(PythonEngineContext context, Click c);
    public void OnResponse(PythonEngineContext context, Message msg);
    public void OnAssist(PythonEngineContext context, string script);
    public void OnFocus(PythonEngineContext context);
    public void OnLoseFocus(PythonEngineContext context);
    public void OnKeyUp(PythonEngineContext context, string key);
    public bool OnSubneg(PythonEngineContext context, byte code, byte[] data);
    public bool OnLine(PythonEngineContext context, string line, string ansi);
    public void OnAfterLine(PythonEngineContext context, string line, string ansi);
    public bool OnSend(PythonEngineContext context, string message);
}
public partial class PythonScriptService : IPythonScriptService
{
    public void InstallTo(PythonEngineContext context)
    {
        initPyAPI(context);
    }
    public void handleError(PythonEngineContext context, Exception ex)
    {
        context.World.HandleScriptError(ex);
        if (ex is PythonException pyEx)
        {
            context.World.DoPrintSystem($"[Script Error] {pyEx.Message}");
        }
    }
    public void Open(PythonEngineContext context)
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
        context.Events.OnLine = data.OnLine;
        context.Events.OnAfterLine = data.OnAfterLine;
        context.Events.OnSend = data.OnSend;
        var entry = Path.Combine(context.World.GetPluginOptions().Location, "main.py");
        var entrydata = File.ReadAllText(entry);
        using (Python.Runtime.Py.GIL())
        {

            try
            {
                context.Scope.Exec(entrydata);
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
    }
    private PyObject? callByName(PythonEngineContext context, string funcname, params PyObject[] args)
    {
        try
        {
            var func = context.Scope.Eval(funcname);
            if (func is not null && func is Python.Runtime.PyObject pyFunc)
            {
                return pyFunc.Invoke(args);
            }

        }
        catch (Exception ex)
        {
            handleError(context, ex);
        }
        return null;
    }
    public void Run(PythonEngineContext context, string script)
    {
        using (Python.Runtime.Py.GIL())
        {
            try
            {
                context.Scope.Exec(script);
            }
            catch (Exception ex)
            {
                handleError(context, ex);
            }
        }
    }
    public void Close(PythonEngineContext context)
    {
        using (Python.Runtime.Py.GIL())
        {

            if (context.Events.OnClose != "")
            {

                callByName(context, context.Events.OnClose);
            }
            context.Scope.Dispose();
        }

    }
    public void OnConnect(PythonEngineContext context)
    {
        if (context.Events.OnConnect != "")
        {
            using (Python.Runtime.Py.GIL())
            {

                callByName(context, context.Events.OnConnect);
            }
        }
    }
    public void OnDisconnect(PythonEngineContext context)
    {
        if (context.Events.OnDisconnect != "")
        {
            using (Python.Runtime.Py.GIL())
            {
                callByName(context, context.Events.OnDisconnect);
            }
        }
    }
    public void OnTrigger(PythonEngineContext context, Line line, Trigger trigger, MatchResult matchResult)
    {
        if (trigger.Script == "")
        {
            return;
        }
        using var model = new PyDict();
        if (model == null)
        {
            return;
        }
        using (Python.Runtime.Py.GIL())
        {
            foreach (var kv in matchResult.Named)
            {
                model[kv.Key] = kv.Value.ToPython();
            }
            for (var k = 0; k < matchResult.List.Count; k++)
            {
                switch (k)
                {
                    case 0:
                        model["10"] = matchResult.List[k].ToPython();
                        break;
                    case > 9:
                        break;

                }
                model[$"{(k - 1).ToString()}"] = matchResult.List[k].ToPython();
            }
            callByName(context, trigger.Script, trigger.Name.ToPython(), line.ToPlainText().ToPython(), model);
        }
    }
    public void OnAlias(PythonEngineContext context, string message, Alias alias, MatchResult matchResult)
    {
        if (alias.Script == "")
        {
            return;
        }
        using var model = new PyDict();
        if (model == null)
        {
            return;
        }
        using (Python.Runtime.Py.GIL())
        {

            foreach (var kv in matchResult.Named)
            {
                model[kv.Key] = kv.Value.ToPython();
            }
            for (var k = 0; k < matchResult.List.Count; k++)
            {
                switch (k)
                {
                    case 0:
                        model["10"] = matchResult.List[k].ToPython();
                        break;
                    case > 9:
                        break;

                }
                model[$"{(k - 1).ToString()}"] = matchResult.List[k].ToPython();
            }
            callByName(context, alias.Script, alias.Name.ToPython(), message.ToPython(), model);
        }
    }
    public void OnTimer(PythonEngineContext context, World.Types.Timer timer)
    {
        using (Python.Runtime.Py.GIL())
        {
            callByName(context, timer.Script, timer.Name.ToPython());
        }
    }
    public void OnCallback(PythonEngineContext context, Callback cb)
    {
        using (Python.Runtime.Py.GIL())
        {
            callByName(context, cb.Script, cb.Name.ToPython(), cb.ID.ToPython(), cb.Code.ToPython(), cb.Data.ToPython());
        }
    }
    public void OnBroadCast(PythonEngineContext context, Broadcast bc)
    {
        using (Python.Runtime.Py.GIL())
        {
            callByName(context, context.Events.OnBroadcast, bc.Message.ToPython(), bc.Global.ToPython(), bc.Channel.ToPython(), bc.ID.ToPython());
        }
    }
    public void OnHUDClick(PythonEngineContext context, Click c)
    {
        using (Python.Runtime.Py.GIL())
        {
            callByName(context, context.Events.OnHUDClick, c.X.ToPython(), c.Y.ToPython());
        }
    }
    public void OnResponse(PythonEngineContext context, Message msg)
    {
        using (Python.Runtime.Py.GIL())
        {
            callByName(context, context.Events.OnResponse, msg.Type.ToPython(), msg.ID.ToPython(), msg.Data.ToPython());
        }
    }
    public void OnAssist(PythonEngineContext context, string script)
    {
        using (Python.Runtime.Py.GIL())
        {
            callByName(context, script);
        }
    }
    public void OnFocus(PythonEngineContext context)
    {
        if (context.Events.OnFocus != "")
        {
            using (Python.Runtime.Py.GIL())
            {
                callByName(context, context.Events.OnFocus);
            }
        }
    }
    public void OnLoseFocus(PythonEngineContext context)
    {
        if (context.Events.OnLoseFocus != "")
        {
            using (Python.Runtime.Py.GIL())
            {
                callByName(context, context.Events.OnLoseFocus);
            }
        }
    }
    public void OnKeyUp(PythonEngineContext context, string key)
    {
        if (context.Events.OnKeyUp != "")
        {
            using (Python.Runtime.Py.GIL())
            {
                callByName(context, context.Events.OnKeyUp, key.ToPython());
            }
        }
    }
    public bool OnSubneg(PythonEngineContext context, byte code, byte[] data)
    {
        if (context.Events.OnSubneg == "")
        {
            return false;
        }
        using (Python.Runtime.Py.GIL())
        {
            return callByName(context, context.Events.OnSubneg, code.ToPython(), data.ToPython())?.IsTrue() ?? false;
        }
    }
    public bool OnLine(PythonEngineContext context, string line, string ansi)
    {
        if (context.Events.OnLine == "")
        {
            return false;
        }
        using (Python.Runtime.Py.GIL())
        {
            return callByName(context, context.Events.OnLine, line.ToPython(), ansi.ToPython())?.IsTrue() ?? false;
        }
    }
    public void OnAfterLine(PythonEngineContext context, string line, string ansi)
    {
        if (context.Events.OnAfterLine != "")
        {
            using (Python.Runtime.Py.GIL())
            {
                callByName(context, context.Events.OnAfterLine, line.ToPython(), ansi.ToPython());
            }
        }
    }
    public bool OnSend(PythonEngineContext context, string message)
    {
        if (context.Events.OnSend == "")
        {
            return false;
        }
        using (Python.Runtime.Py.GIL())
        {
            return callByName(context, context.Events.OnSend, message.ToPython())?.IsTrue() ?? false;
        }
    }

}