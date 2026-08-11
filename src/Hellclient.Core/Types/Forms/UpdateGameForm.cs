namespace Hellclient.Core.Types.Forms;

public class UpdateGameForm
{
    public string Name { get; set; } = "";
    public string ID { get; set; } = "";
    public string Host { get; set; } = "";
    public string Port { get; set; } = "";
    public string Charset { get; set; } = "";
    public string ScriptPrefix { get; set; } = "";
    public string CommandStackCharacter { get; set; } = "";
    public string Proxy { get; set; } = "";
    public bool ShowBroadcast { get; set; } = false;
    public bool ShowSubneg { get; set; } = false;
    public bool ModEnabled { get; set; } = false;
    public bool AutoSave { get; set; } = false;
    public bool IgnoreBatchCommand { get; set; } = false;

}