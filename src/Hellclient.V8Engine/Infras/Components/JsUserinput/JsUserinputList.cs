using System.Dynamic;
using Hellclient.Script.Helpers;
using Hellclient.Script.Types.Userinput;
using Hellclient.World.Cores;

namespace Hellclient.V8Engine.Infras.Components.JsUserinput;

public class JsUserinputList(IWorld world, DataList dataList)
{
    private readonly IWorld _world = world;
    private readonly DataList _dataList = dataList;
    public Object? Publish(params object[] args)
    {
        var ui = UserinputHelper.SendList(_world, JsAPI.GetStringArg(args, 0), _dataList);
        return null;
    }
    public Object? Append(params object[] args)
    {
        _dataList.Append(JsAPI.GetStringArg(args, 0), JsAPI.GetStringArg(args, 1));
        return null;
    }
    public Object? SetValues(params object[] args)
    {
        _dataList.SetValues(JsAPI.GetStringArrayArg(args, 0));
        return null;
    }
    public Object? SetMulti(params object[] args)
    {
        _dataList.SetMulti(JsAPI.GetBoolArg(args, 0));
        return null;
    }
    public Object? Convert()
    {
#pragma warning disable CS8974
        var result = new ExpandoObject() as IDictionary<string, object>;
        result["Append"] = Append;
        result["Publish"] = Publish;
        result["SetValues"] = SetValues;
        result["SetMulti"] = SetMulti;
        result["SetMutli"] = SetMulti;//backwards compatibility

        result["append"] = Append;
        result["publish"] = Publish;
        result["setvalues"] = SetValues;
        result["setmulti"] = SetMulti;
        result["setmutli"] = SetMulti;//backwards compatibility
        return result;
#pragma warning restore CS8974
    }
}