using Python.Runtime;
using Hellclient.Script.Helpers;
using Hellclient.Script.Types.Userinput;
using Hellclient.World.Cores;

namespace Hellclient.PythonEngine.Infras.Components.PyUserinput;

public class UserinputLIst
{

}
public class PyUserinputList(IWorld world, DataList dataList)
{
    private readonly IWorld _world = world;
    private readonly DataList _dataList = dataList;
    public PyObject? Publish(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var ui = UserinputHelper.SendList(_world, PyAPI.GetStringArg(args, 0), _dataList);
        return null;
    }
    public PyObject? Append(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _dataList.Append(PyAPI.GetStringArg(args, 0), PyAPI.GetStringArg(args, 1));
        return null;
    }
    public PyObject? SetValues(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _dataList.SetValues(PyAPI.GetStringArrayArg(args, 0));
        return null;
    }
    public PyObject? SetMulti(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _dataList.SetMulti(PyAPI.GetBoolArg(args, 0));
        return null;
    }
    public PyObject Convert()
    {
#pragma warning disable CS8974
        var result = (new UserinputLIst()).ToPython();
        result.SetAttr("Append", PyObject.FromManagedObject(Append));
        result.SetAttr("Publish", PyObject.FromManagedObject(Publish));
        result.SetAttr("SetValues", PyObject.FromManagedObject(SetValues));
        result.SetAttr("SetMulti", PyObject.FromManagedObject(SetMulti));
        result.SetAttr("SetMutli", PyObject.FromManagedObject(SetMulti));//backwards compatibility

        result.SetAttr("append", PyObject.FromManagedObject(Append));
        result.SetAttr("publish", PyObject.FromManagedObject(Publish));
        result.SetAttr("setvalues", PyObject.FromManagedObject(SetValues));
        result.SetAttr("setmulti", PyObject.FromManagedObject(SetMulti));
        result.SetAttr("setmutli", PyObject.FromManagedObject(SetMulti));//backwards compatibility
        return result;
#pragma warning restore CS8974
    }
}