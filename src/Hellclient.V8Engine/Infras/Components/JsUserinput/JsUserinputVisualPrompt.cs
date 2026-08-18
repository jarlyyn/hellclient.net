using Hellclient.World.Cores;
using Hellclient.Script.Types.Userinput;
using Hellclient.Script.Helpers;
using System.Dynamic;

namespace Hellclient.V8Engine.Infras.Components.JsUserinput;

public class JsUserinputVisualPrompt(IWorld world, VisualPrompt visualPrompt)
{
    private readonly IWorld _world = world;
    private readonly VisualPrompt _visualPrompt = visualPrompt;
    public Object? Publish(params object[] args)
    {
        var ui = UserinputHelper.SendVisualPrompt(_world, JsAPI.GetStringArg(args, 0), _visualPrompt);
        return null;
    }
    public Object? SetMediaType(params object[] args)
    {
        _visualPrompt.SetMediaType(JsAPI.GetStringArg(args, 0));
        return null;
    }
    public Object? SetPortrait(params object[] args)
    {
        _visualPrompt.SetPortrait(JsAPI.GetBoolArg(args, 0));
        return null;
    }
    public Object? SetValue(params object[] args)
    {
        _visualPrompt.SetValue(JsAPI.GetStringArg(args, 0));
        return null;
    }
    public Object? SetRefreshCallback(params object[] args)
    {
        _visualPrompt.SetRefreshCallback(JsAPI.GetStringArg(args, 0));
        return null;
    }
    public Object? Append(params object[] args)
    {
        _visualPrompt.Append(JsAPI.GetStringArg(args, 0), JsAPI.GetStringArg(args, 1));
        return null;
    }
    public Object? Convert()
    {
#pragma warning disable CS8974
        var result = new ExpandoObject() as IDictionary<string, object>;
        result["Publish"] = Publish;
        result["SetMediaType"] = SetMediaType;
        result["SetPortrait"] = SetPortrait;
        result["SetValue"] = SetValue;
        result["SetRefreshCallback"] = SetRefreshCallback;
        result["Append"] = Append;

        result["publish"] = Publish;
        result["setmediatype"] = SetMediaType;
        result["setportrait"] = SetPortrait;
        result["setvalue"] = SetValue;
        result["setrefreshcallback"] = SetRefreshCallback;
        result["append"] = Append;

        return result;
#pragma warning restore CS8974
    }
}