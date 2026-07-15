namespace Hellclient.World.Models;

public class WalkAllResult
{
    public List<Step> Steps { get; set; } = [];
    public List<string> Walked { get; set; } = [];
    public List<string> NotWalked { get; set; } = [];

}