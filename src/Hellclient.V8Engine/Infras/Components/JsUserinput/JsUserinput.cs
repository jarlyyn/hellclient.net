using System.Dynamic;
using Hellclient.Script.Helpers;
using Hellclient.Script.Types.Userinput;
using Hellclient.World.Cores;
using Microsoft.ClearScript.V8;

namespace Hellclient.V8Engine.Infras.Components.JsUserinput;

public class JsUserinput(IWorld world, V8ScriptEngine engine)
{

    private readonly IWorld _world = world;
    private readonly V8ScriptEngine _engine = engine;
    public Object? HideAll(params object[] args)
    {
        UserinputHelper.HideAll(_world);
        return null;
    }
    public Object? Prompt(params object[] args)
    {
        var ui = UserinputHelper.SendPrompt(
            _world,
            JsAPI.GetStringArg(args, 0),
            JsAPI.GetStringArg(args, 1),
            JsAPI.GetStringArg(args, 2),
            JsAPI.GetStringArg(args, 3)
        );
        return ui.ID;
    }
    public Object? Confirm(params object[] args)
    {
        var ui = UserinputHelper.SendConfirm(
            _world,
            JsAPI.GetStringArg(args, 0),
            JsAPI.GetStringArg(args, 1),
            JsAPI.GetStringArg(args, 2)
        );
        return ui.ID;
    }
    public Object? Alert(params object[] args)
    {
        var ui = UserinputHelper.SendAlert(
            _world,
            JsAPI.GetStringArg(args, 0),
            JsAPI.GetStringArg(args, 1),
            JsAPI.GetStringArg(args, 2)
        );
        return ui.ID;
    }
    public Object? Popup(params object[] args)
    {
        var ui = UserinputHelper.SendPopup(
            _world,
            JsAPI.GetStringArg(args, 0),
            JsAPI.GetStringArg(args, 1),
            JsAPI.GetStringArg(args, 2),
            JsAPI.GetStringArg(args, 3)
        );
        return ui.ID;
    }
    public Object? Note(params object[] args)
    {
        var ui = UserinputHelper.SendNote(
            _world,
            JsAPI.GetStringArg(args, 0),
            JsAPI.GetStringArg(args, 1),
            JsAPI.GetStringArg(args, 2),
            JsAPI.GetStringArg(args, 3)
        );
        return ui.ID;
    }
    public Object? Custom(params object[] args)
    {
        var ui = UserinputHelper.SendCustom(
            _world,
            JsAPI.GetStringArg(args, 0),
            JsAPI.GetStringArg(args, 1),
            JsAPI.GetStringArg(args, 2)
        );
        return ui.ID;
    }
    public Object? NewList(params object[] args)
    {
        var list = DataList.Create(JsAPI.GetStringArg(args, 0), JsAPI.GetStringArg(args, 1), JsAPI.GetBoolArg(args, 2));
        return new JsUserinputList(_world, _engine, list).Convert();
    }
    public Object? NewDatagrid(params object[] args)
    {
        var grid = Datagrid.Create(JsAPI.GetStringArg(args, 0), JsAPI.GetStringArg(args, 1));
        return new JsUserinputDataGrid(_world, _engine, grid).Convert();
    }
    public Object? NewVisualPrompt(params object[] args)
    {
        var visualPrompt = VisualPrompt.Create(JsAPI.GetStringArg(args, 0), JsAPI.GetStringArg(args, 1), JsAPI.GetStringArg(args, 2));
        return new JsUserinputVisualPrompt(_world, _engine, visualPrompt).Convert();
    }
    public Object? Convert(params object[] args)
    {
#pragma warning disable CS8974
        var result = _engine.Evaluate("({})") as Microsoft.ClearScript.ScriptObject;
        if (result is null)
        {
            throw new Exception("Failed to create script object");
        }
        result["HideAll"] = HideAll;
        result["Prompt"] = Prompt;
        result["Confirm"] = Confirm;
        result["Alert"] = Alert;
        result["Popup"] = Popup;
        result["Note"] = Note;
        result["Custom"] = Custom;
        result["NewList"] = NewList;
        result["NewDatagrid"] = NewDatagrid;
        result["NewVisualPrompt"] = NewVisualPrompt;

        result["hideall"] = HideAll;
        result["prompt"] = Prompt;
        result["confirm"] = Confirm;
        result["alert"] = Alert;
        result["popup"] = Popup;
        result["note"] = Note;
        result["custom"] = Custom;
        result["newlist"] = NewList;
        result["newdatagrid"] = NewDatagrid;
        result["newvisualprompt"] = NewVisualPrompt;
#pragma warning restore CS8974
        return result;
    }
}