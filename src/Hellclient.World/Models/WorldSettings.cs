namespace Hellclient.World.Models;

public class WorldSettings
{
    public string ID { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public string Port { get; set; } = string.Empty;
    public string Proxy { get; set; } = string.Empty;
    public string Charset { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CommandStackCharacter { get; set; } = string.Empty;
    public string ScriptPrefix { get; set; } = string.Empty;
    public bool ShowBroadcast { get; set; } = false;
    public bool ShowSubneg { get; set; } = false;
    public bool ModEnabled { get; set; } = false;
    public bool AutoSave { get; set; } = false;
    public bool IgnoreBatchCommand { get; set; } = false;
 
}