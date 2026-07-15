namespace Hellclient.World.Models;

public class Path
{
    public string Command { get; set; } = string.Empty;
    public int Delay { get; set; } = 0;
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public Dictionary<string, bool> Tags { get; set; } = [];
    public Dictionary<string, bool> ExcludeTags { get; set; } = [];

}