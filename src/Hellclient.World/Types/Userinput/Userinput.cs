using Hellclient.World.Infras.Adapters;

namespace Hellclient.Script.Types.Userinput;

public class Userinput
{
    public const string NameList = "userinput.list";
    public const string NameHideList = "userinput.hidelist";
    public const string NameAlert = "userinput.alert";
    public const string NameNote = "userinput.note";
    public const string NameConfirm = "userinput.confirm";
    public const string NamePrompt = "userinput.prompt";
    public const string NamePopup = "userinput.popup";
    public const string NameDatagrid = "userinput.datagrid";
    public const string NameHideDatagrid = "userinput.hidedatagrid";
    public const string NameVisualPrompt = "userinput.visualprompt";
    public const string NameHideall = "userinput.hideall";
    public const string NameCustom = "userinput.custom";

    public string Name { get; set; } = "";
    public string Script { get; set; } = "";
    public string ID { get; set; } = "";
    public object? Data { get; set; } = null;
    public static Userinput Create(string name, string script, object? data)
    {
        return new Userinput
        {
            Name = name,
            Script = script,
            ID = SimpleID.Instance.GenerateID(),
            Data = data
        };
    }
}