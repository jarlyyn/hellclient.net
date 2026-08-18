using Hellclient.Script.Types.Userinput;
using Hellclient.World.Cores;

namespace Hellclient.Script.Helpers;

public class UserinputHelper
{
    public static Userinput SendConfirm(IWorld world, string script, string title, string intro)
    {
        var data = new Dictionary<string, object>()
        {
            ["Title"] = title,
            ["Intro"] = intro
        };
        var ui = Userinput.Create(Userinput.NameConfirm, script, data);
        world.EventBus.ScriptMessageEvent?.Invoke(world, ui);
        return ui;
    }
    public static Userinput SendAlert(IWorld world, string script, string title, string intro)
    {
        var data = new Dictionary<string, object>()
        {
            ["Title"] = title,
            ["Intro"] = intro
        };
        var ui = Userinput.Create(Userinput.NameAlert, script, data);
        world.EventBus.ScriptMessageEvent?.Invoke(world, ui);
        return ui;
    }
    public static Userinput SendNote(IWorld world, string script, string title, string body, string notetype)
    {
        var data = new Dictionary<string, object>()
        {
            ["Title"] = title,
            ["Body"] = body,
            ["Type"] = notetype
        };
        var ui = Userinput.Create(Userinput.NameNote, script, data);
        world.EventBus.ScriptMessageEvent?.Invoke(world, ui);
        return ui;
    }
    public static Userinput SendPrompt(IWorld world, string script, string title, string intro, string value)
    {
        var data = new Dictionary<string, object>()
        {
            ["Title"] = title,
            ["Intro"] = intro,
            ["Value"] = value
        };
        var ui = Userinput.Create(Userinput.NamePrompt, script, data);
        world.EventBus.ScriptMessageEvent?.Invoke(world, ui);
        return ui;
    }
    public static Userinput SendPopup(IWorld world, string script, string title, string intro, string popuptype)
    {
        var data = new Dictionary<string, object>()
        {
            ["Title"] = title,
            ["Intro"] = intro,
            ["Type"] = popuptype
        };
        var ui = Userinput.Create(Userinput.NamePopup, script, data);
        world.EventBus.ScriptMessageEvent?.Invoke(world, ui);
        return ui;
    }
    public static void HideAll(IWorld world)
    {
        var ui = Userinput.Create(Userinput.NameHideall, "", null);
        world.EventBus.ScriptMessageEvent?.Invoke(world, ui);
    }
    public static Userinput SendCustom(IWorld world, string script, string customtype, string value)
    {
        var data = new Dictionary<string, object>()
        {
            ["Type"] = customtype,
            ["Value"] = value
        };
        var ui = Userinput.Create(Userinput.NameCustom, script, data);
        world.EventBus.ScriptMessageEvent?.Invoke(world, ui);
        return ui;
    }
    public static Userinput SendDatagrid(IWorld world, string script, Datagrid datagrid)
    {
        var ui = Userinput.Create(Userinput.NameDatagrid, script, datagrid);
        world.EventBus.ScriptMessageEvent?.Invoke(world, ui);
        return ui;
    }
    public static Userinput SendVisualPrompt(IWorld world, string script, VisualPrompt visualprompt)
    {
        var ui = Userinput.Create(Userinput.NameVisualPrompt, script, visualprompt);
        world.EventBus.ScriptMessageEvent?.Invoke(world, ui);
        return ui;
    }
    public static Userinput SendList(IWorld world, string script, DataList datalist)
    {
        var ui = Userinput.Create(Userinput.NameList, script, datalist);
        world.EventBus.ScriptMessageEvent?.Invoke(world, ui);
        return ui;
    }
}
