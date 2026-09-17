using Hellclient.World.Cores;
using Hellclient.Script.Types.Userinput;
using Hellclient.Script.Helpers;
using Python.Runtime;

namespace Hellclient.PythonEngine.Infras.Components.PyUserinput;

public class UserInputDataGrid
{

}
public class PyUserinputDataGrid(IWorld world, Datagrid dataGrid)
{
    private readonly IWorld _world = world;

    private readonly Datagrid _dataGrid = dataGrid;
    public PyObject? SetPage(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _dataGrid.SetPage(PyAPI.GetIntArg(args, 0));
        return null;
    }
    public PyObject? GetPage(params PyObject[] args)
    {
        using var _ = Py.GIL();        
        return _dataGrid.GetPage().ToPython();
    }
    public PyObject? SetMaxPage(params PyObject[] args)
    {
        using var _ = Py.GIL();        
        _dataGrid.SetMaxPage(PyAPI.GetIntArg(args, 0));
        return null;
    }
    public PyObject? SetFilter(params PyObject[] args)
    {
        using var _ = Py.GIL();        
        _dataGrid.SetFilter(PyAPI.GetStringArg(args, 0));
        return null;
    }
    public PyObject? GetFilter(params PyObject[] args)
    {
        using var _ = Py.GIL();        
        return _dataGrid.GetFilter().ToPython();
    }
    public PyObject? SetOnPage(params PyObject[] args)
    {
        using var _ = Py.GIL();        
        _dataGrid.SetOnPage(PyAPI.GetStringArg(args, 0));
        return null;
    }
    public PyObject? SetOnFilter(params PyObject[] args)
    {
        using var _ = Py.GIL();        
        _dataGrid.SetOnFilter(PyAPI.GetStringArg(args, 0));
        return null;
    }
    public PyObject? SetOnDelete(params PyObject[] args)
    {
        using var _ = Py.GIL();        
        _dataGrid.SetOnDelete(PyAPI.GetStringArg(args, 0));
        return null;
    }
    public PyObject? SetOnView(params PyObject[] args)
    {
        using var _ = Py.GIL();        
        _dataGrid.SetOnView(PyAPI.GetStringArg(args, 0));
        return null;
    }
    public PyObject? SetOnSelect(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _dataGrid.SetOnSelect(PyAPI.GetStringArg(args, 0));
        return null;
    }
    public PyObject? SetOnCreate(params PyObject[] args)
    {
        using var _ = Py.GIL();        
        _dataGrid.SetOnCreate(PyAPI.GetStringArg(args, 0));
        return null;
    }
    public PyObject? SetOnUpdate(params PyObject[] args)
    {
        using var _ = Py.GIL();        
        _dataGrid.SetOnUpdate(PyAPI.GetStringArg(args, 0));
        return null;
    }
    public PyObject? ResetItems(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _dataGrid.ResetItems();
        return null;
    }
    public PyObject? Append(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _dataGrid.Append(PyAPI.GetStringArg(args, 0), PyAPI.GetStringArg(args, 1));
        return null;
    }
    public PyObject? Publish(params PyObject[] args)
    {
        using var _ = Py.GIL();        
        var ui = UserinputHelper.SendDatagrid(_world, PyAPI.GetStringArg(args, 0), _dataGrid);
        return ui.ID.ToPython();
    }
    public PyObject? Hide()
    {
        using var _ = Py.GIL();        
        UserinputHelper.HideAll(_world);
        return null;
    }
    public PyObject Convert()
    {
#pragma warning disable CS8974
        var result = new UserInputDataGrid().ToPython();
        result.SetAttr("SetPage", PyObject.FromManagedObject(SetPage));
        result.SetAttr("GetPage", PyObject.FromManagedObject(GetPage));
        result.SetAttr("SetMaxPage", PyObject.FromManagedObject(SetMaxPage));
        result.SetAttr("SetFilter", PyObject.FromManagedObject(SetFilter));
        result.SetAttr("GetFilter", PyObject.FromManagedObject(GetFilter));
        result.SetAttr("SetOnPage", PyObject.FromManagedObject(SetOnPage));
        result.SetAttr("SetOnFilter", PyObject.FromManagedObject(SetOnFilter));
        result.SetAttr("SetOnDelete", PyObject.FromManagedObject(SetOnDelete));
        result.SetAttr("SetOnView", PyObject.FromManagedObject(SetOnView));
        result.SetAttr("SetOnSelect", PyObject.FromManagedObject(SetOnSelect));
        result.SetAttr("SetOnCreate", PyObject.FromManagedObject(SetOnCreate));
        result.SetAttr("SetOnUpdate", PyObject.FromManagedObject(SetOnUpdate));
        result.SetAttr("ResetItems", PyObject.FromManagedObject(ResetItems));
        result.SetAttr("Append", PyObject.FromManagedObject(Append));
        result.SetAttr("Publish", PyObject.FromManagedObject(Publish));
        result.SetAttr("Hide", PyObject.FromManagedObject(Hide));

        result.SetAttr("setpage", PyObject.FromManagedObject(SetPage));
        result.SetAttr("getpage", PyObject.FromManagedObject(GetPage));
        result.SetAttr("setmaxpage", PyObject.FromManagedObject(SetMaxPage));
        result.SetAttr("setfilter", PyObject.FromManagedObject(SetFilter));
        result.SetAttr("getfilter", PyObject.FromManagedObject(GetFilter));
        result.SetAttr("setonpage", PyObject.FromManagedObject(SetOnPage));
        result.SetAttr("setonfilter", PyObject.FromManagedObject(SetOnFilter));
        result.SetAttr("setondelete", PyObject.FromManagedObject(SetOnDelete));
        result.SetAttr("setonview", PyObject.FromManagedObject(SetOnView));
        result.SetAttr("setonselect", PyObject.FromManagedObject(SetOnSelect));
        result.SetAttr("setoncreate", PyObject.FromManagedObject(SetOnCreate));
        result.SetAttr("setonupdate", PyObject.FromManagedObject(SetOnUpdate));
        result.SetAttr("resetitems", PyObject.FromManagedObject(ResetItems));
        result.SetAttr("append", PyObject.FromManagedObject(Append));
        result.SetAttr("publish", PyObject.FromManagedObject(Publish));
        result.SetAttr("hide", PyObject.FromManagedObject(Hide));
#pragma warning restore CS8974
        return result;
    }
}
