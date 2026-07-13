namespace Hellclient.Core.Models;

public class ClientInfo
{
    public string ID { get; set; } = string.Empty;
    public long ReadyAt { get; set; } = 0;
    public int Position { get; set; } = 0;
    public string HostPort { get; set; } = string.Empty;
    public string ScriptID { get; set; } = string.Empty;
    public bool Running { get; set; } = false;
    public int Priority { get; set; } = 0;
    public long LastActive { get; set; } = 0;
    public List<Line> Summary { get; set; } = new List<Line>();

}