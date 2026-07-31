namespace Hellclient.World.Types;

public class BatchCommand
{
    public List<string> Scripts { get; set; } = [];
    public string Command { get; set; } = "";
}