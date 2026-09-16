using Hellclient.World.Cores;
using Hellclient.Script.Types.Userinput;
using Hellclient.Script.Helpers;
using Python.Runtime;

namespace Hellclient.PythonEngine.Infras.Components.PyUserinput;
public class UserinputVisualPrompt
{

}
public class PyUserinputVisualPrompt(IWorld world,VisualPrompt visualPrompt)
{
    private readonly IWorld _world = world;
    private readonly VisualPrompt _visualPrompt = visualPrompt;
    public PyObject? Publish(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var ui = UserinputHelper.SendVisualPrompt(_world, PyAPI.GetStringArg(args, 0), _visualPrompt);
        return null;
    }
    public PyObject? SetMediaType(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _visualPrompt.SetMediaType(PyAPI.GetStringArg(args, 0));
        return null;
    }
    public PyObject? SetPortrait(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _visualPrompt.SetPortrait(PyAPI.GetBoolArg(args, 0));
        return null;
    }
    public PyObject? SetValue(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _visualPrompt.SetValue(PyAPI.GetStringArg(args, 0));
        return null;
    }
    public PyObject? SetRefreshCallback(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _visualPrompt.SetRefreshCallback(PyAPI.GetStringArg(args, 0));
        return null;
    }
    public PyObject? Append(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _visualPrompt.Append(PyAPI.GetStringArg(args, 0), PyAPI.GetStringArg(args, 1));
        return null;
    }
    public PyObject Convert()
    {
#pragma warning disable CS8974
        var result = (new UserinputVisualPrompt()).ToPython();
        result.SetAttr("Publish", PyObject.FromManagedObject(Publish));
        result.SetAttr("SetMediaType", PyObject.FromManagedObject(SetMediaType));
        result.SetAttr("SetPortrait", PyObject.FromManagedObject(SetPortrait));
        result.SetAttr("SetValue", PyObject.FromManagedObject(SetValue));
        result.SetAttr("SetRefreshCallback", PyObject.FromManagedObject(SetRefreshCallback));
        result.SetAttr("Append", PyObject.FromManagedObject(Append));

        result.SetAttr("publish", PyObject.FromManagedObject(Publish));
        result.SetAttr("setmediatype", PyObject.FromManagedObject(SetMediaType));
        result.SetAttr("setportrait", PyObject.FromManagedObject(SetPortrait));
        result.SetAttr("setvalue", PyObject.FromManagedObject(SetValue));
        result.SetAttr("setrefreshcallback", PyObject.FromManagedObject(SetRefreshCallback));
        result.SetAttr("append", PyObject.FromManagedObject(Append));

        return result;
#pragma warning restore CS8974
    }
}