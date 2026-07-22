using Hellclient.World.Types;
using Path = Hellclient.World.Types.Path;

namespace Hellclient.World.States;

public class WalkContext
{
    public Dictionary<string, bool> Tags { get; set; } = [];
    public Dictionary<string, Room> Rooms { get; set; } = [];
    public string From { get; set; } = string.Empty;
    public List<string> To { get; set; } = [];
    public List<Path> Fly { get; set; } = [];
    public Dictionary<string, Step> Walked { get; set; } = [];
    
    // 	forwading   *list.List
    public List<Step> Forwarding { get; set; } = [];
    public Dictionary<string, bool> Blacklist { get; set; } = [];
    public Dictionary<string, bool> Whitelist { get; set; } = [];
    public Dictionary<string, Dictionary<string, bool>> BlockedPath { get; set; } = [];
    public int MaxDistance { get; set; }
}
