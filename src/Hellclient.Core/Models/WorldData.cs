namespace Hellclient.Core.Models;

public class WorldData
{
    public string Host { get; set; } = string.Empty;
    public string Port { get; set; } = string.Empty;
    public string Charset { get; set; } = string.Empty;
    public string Proxy { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CommandStackCharacter { get; set; } = string.Empty;
    public string ScriptPrefix { get; set; } = string.Empty;
    public int QueueDelay { get; set; } = 0;
    public Dictionary<string, string> Params { get; set; } = new Dictionary<string, string>();
    public Dictionary<string, string> ParamComments { get; set; } = new Dictionary<string, string>();
    public List<string> Permissions { get; set; } = new List<string>();
    public string ScriptID { get; set; } = string.Empty;
    public bool ShowBroadcast { get; set; } = false;
    public bool ShowSubneg { get; set; } = false;
    public bool ModEnabled { get; set; } = false;
    public Trusted Trusted { get; set; } = new Trusted();
    public List<Trigger> Triggers { get; set; } = new List<Trigger>();
    public List<Timer> Timers { get; set; } = new List<Timer>();
    public List<Alias> Aliases { get; set; } = new List<Alias>();
    public bool AutoSave { get; set; } = false;
    public bool IgnoreBatchCommand { get; set; } = false;

}