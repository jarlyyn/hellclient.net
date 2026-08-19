using Hellclient.World.Cores;
using Hellclient.Script.Types.Userinput;
using Hellclient.Script.Helpers;
using System.Dynamic;
using Microsoft.ClearScript.V8;

namespace Hellclient.V8Engine.Infras.Components.JsUserinput;

public class JsUserinputDataGrid(IWorld world, V8ScriptEngine engine,Datagrid dataGrid)
{
    private readonly IWorld _world = world;
    private readonly V8ScriptEngine _engine = engine;
    private readonly Datagrid _dataGrid = dataGrid;
    public Object? SetPage(params object[] args)
    {
        _dataGrid.SetPage(JsAPI.GetIntArg(args, 0));
        return null;
    }
    public Object? GetPage(params object[] args)
    {
        return _dataGrid.GetPage();
    }
    public Object? SetMaxPage(params object[] args)
    {
        _dataGrid.SetMaxPage(JsAPI.GetIntArg(args, 0));
        return null;
    }
    public Object? SetFilter(params object[] args)
    {
        _dataGrid.SetFilter(JsAPI.GetStringArg(args, 0));
        return null;
    }
    public Object? GetFilter(params object[] args)
    {
        return _dataGrid.GetFilter();
    }
    public Object? SetOnPage(params object[] args)
    {
        _dataGrid.SetOnPage(JsAPI.GetStringArg(args, 0));
        return null;
    }
    public Object? SetOnFilter(params object[] args)
    {
        _dataGrid.SetOnFilter(JsAPI.GetStringArg(args, 0));
        return null;
    }
    public Object? SetOnDelete(params object[] args)
    {
        _dataGrid.SetOnDelete(JsAPI.GetStringArg(args, 0));
        return null;
    }
    public Object? SetOnView(params object[] args)
    {
        _dataGrid.SetOnView(JsAPI.GetStringArg(args, 0));
        return null;
    }
    public Object? SetOnSelect(params object[] args)
    {
        _dataGrid.SetOnSelect(JsAPI.GetStringArg(args, 0));
        return null;
    }
    public Object? SetOnCreate(params object[] args)
    {
        _dataGrid.SetOnCreate(JsAPI.GetStringArg(args, 0));
        return null;
    }
    public Object? SetOnUpdate(params object[] args)
    {
        _dataGrid.SetOnUpdate(JsAPI.GetStringArg(args, 0));
        return null;
    }
    public Object? ResetItems(params object[] args)
    {
        _dataGrid.ResetItems();
        return null;
    }
    public Object? Append(params object[] args)
    {
        _dataGrid.Append(JsAPI.GetStringArg(args, 0), JsAPI.GetStringArg(args, 1));
        return null;
    }
    public Object? Publish(params object[] args)
    {
        var ui = UserinputHelper.SendDatagrid(_world, JsAPI.GetStringArg(args, 0), _dataGrid);
        return ui.ID;
    }
    public Object? Hide()
    {
        UserinputHelper.HideAll(_world);
        return null;
    }
    public Object? Convert()
    {
#pragma warning disable CS8974
        var result = _engine.Evaluate("({})") as Microsoft.ClearScript.ScriptObject;
        if (result is null)
        {
            throw new Exception("Failed to create script object");
        }
        result["SetPage"] = SetPage;
        result["GetPage"] = GetPage;
        result["SetMaxPage"] = SetMaxPage;
        result["SetFilter"] = SetFilter;
        result["GetFilter"] = GetFilter;
        result["SetOnPage"] = SetOnPage;
        result["SetOnFilter"] = SetOnFilter;
        result["SetOnDelete"] = SetOnDelete;
        result["SetOnView"] = SetOnView;
        result["SetOnSelect"] = SetOnSelect;
        result["SetOnCreate"] = SetOnCreate;
        result["SetOnUpdate"] = SetOnUpdate;
        result["ResetItems"] = ResetItems;
        result["Append"] = Append;
        result["Publish"] = Publish;
        result["Hide"] = Hide;

        result["setpage"] = SetPage;
        result["getpage"] = GetPage;
        result["setmaxpage"] = SetMaxPage;
        result["setfilter"] = SetFilter;
        result["getfilter"] = GetFilter;
        result["setonpage"] = SetOnPage;
        result["setonfilter"] = SetOnFilter;
        result["setondelete"] = SetOnDelete;
        result["setonview"] = SetOnView;
        result["setonselect"] = SetOnSelect;
        result["setoncreate"] = SetOnCreate;
        result["setonupdate"] = SetOnUpdate;
        result["resetitems"] = ResetItems;
        result["append"] = Append;
        result["publish"] = Publish;
        result["hide"] = Hide;
#pragma warning restore CS8974
        return result;
    }
}
