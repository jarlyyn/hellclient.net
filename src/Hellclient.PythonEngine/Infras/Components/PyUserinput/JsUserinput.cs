using System.Dynamic;
using Hellclient.Script.Helpers;
using Hellclient.Script.Types.Userinput;
using Hellclient.World.Cores;
using Python.Runtime;

namespace Hellclient.PythonEngine.Infras.Components.PyUserinput;

public class Userinpnut()
{

}
public class PyUserinput(IWorld world)
{

    private readonly IWorld _world = world;
    public PyObject? HideAll(params PyObject[] args)
    {
        using var _ = Py.GIL();
        UserinputHelper.HideAll(_world);
        return null;
    }
    public PyObject? Prompt(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var ui = UserinputHelper.SendPrompt(
            _world,
            PyAPI.GetStringArg(args, 0),
            PyAPI.GetStringArg(args, 1),
            PyAPI.GetStringArg(args, 2),
            PyAPI.GetStringArg(args, 3)
        );
        return ui.ID.ToPython();
    }
    public PyObject? Confirm(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var ui = UserinputHelper.SendConfirm(
            _world,
            PyAPI.GetStringArg(args, 0),
            PyAPI.GetStringArg(args, 1),
            PyAPI.GetStringArg(args, 2)
        );
        return ui.ID.ToPython();
    }
    public PyObject? Alert(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var ui = UserinputHelper.SendAlert(
            _world,
            PyAPI.GetStringArg(args, 0),
            PyAPI.GetStringArg(args, 1),
            PyAPI.GetStringArg(args, 2)
        );
        return ui.ID.ToPython();
    }
    public PyObject? Popup(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var ui = UserinputHelper.SendPopup(
            _world,
            PyAPI.GetStringArg(args, 0),
            PyAPI.GetStringArg(args, 1),
            PyAPI.GetStringArg(args, 2),
            PyAPI.GetStringArg(args, 3)
        );
        return ui.ID.ToPython();
    }
    public PyObject? Note(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var ui = UserinputHelper.SendNote(
            _world,
            PyAPI.GetStringArg(args, 0),
            PyAPI.GetStringArg(args, 1),
            PyAPI.GetStringArg(args, 2),
            PyAPI.GetStringArg(args, 3)
        );
        return ui.ID.ToPython();
    }
    public PyObject? Custom(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var ui = UserinputHelper.SendCustom(
            _world,
            PyAPI.GetStringArg(args, 0),
            PyAPI.GetStringArg(args, 1),
            PyAPI.GetStringArg(args, 2)
        );
        return ui.ID.ToPython();
    }
    public PyObject? NewList(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var list = DataList.Create(PyAPI.GetStringArg(args, 0), PyAPI.GetStringArg(args, 1), PyAPI.GetBoolArg(args, 2));
        return new PyUserinputList(_world, list).Convert();
    }
    public PyObject? NewDatagrid(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var grid = Datagrid.Create(PyAPI.GetStringArg(args, 0), PyAPI.GetStringArg(args, 1));
        return new PyUserinputDataGrid(_world, grid).Convert();
    }
    public PyObject? NewVisualPrompt(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var visualPrompt = VisualPrompt.Create(PyAPI.GetStringArg(args, 0), PyAPI.GetStringArg(args, 1), PyAPI.GetStringArg(args, 2));
        return new PyUserinputVisualPrompt(_world, visualPrompt).Convert();
    }
    public PyObject Convert()
    {
#pragma warning disable CS8974 // 将方法组转换为非委托类型
        var result = (new Userinput()).ToPython();
        result.SetAttr("HideAll", PyObject.FromManagedObject(HideAll));
        result.SetAttr("Prompt", PyObject.FromManagedObject(Prompt));
        result.SetAttr("Confirm", PyObject.FromManagedObject(Confirm));
        result.SetAttr("Alert", PyObject.FromManagedObject(Alert));
        result.SetAttr("Popup", PyObject.FromManagedObject(Popup));
        result.SetAttr("Note", PyObject.FromManagedObject(Note));
        result.SetAttr("Custom", PyObject.FromManagedObject(Custom));
        result.SetAttr("NewList", PyObject.FromManagedObject(NewList));
        result.SetAttr("NewDatagrid", PyObject.FromManagedObject(NewDatagrid));
        result.SetAttr("NewVisualPrompt", PyObject.FromManagedObject(NewVisualPrompt));

        result.SetAttr("hideall", PyObject.FromManagedObject(HideAll));
        result.SetAttr("prompt", PyObject.FromManagedObject(Prompt));
        result.SetAttr("confirm", PyObject.FromManagedObject(Confirm));
        result.SetAttr("alert", PyObject.FromManagedObject(Alert));
        result.SetAttr("popup", PyObject.FromManagedObject(Popup));
        result.SetAttr("note", PyObject.FromManagedObject(Note));
        result.SetAttr("custom", PyObject.FromManagedObject(Custom));
        result.SetAttr("newlist", PyObject.FromManagedObject(NewList));
        result.SetAttr("newdatagrid", PyObject.FromManagedObject(NewDatagrid));
        result.SetAttr("newvisualprompt", PyObject.FromManagedObject(NewVisualPrompt));
#pragma warning restore CS8974
        return result;
    }
}