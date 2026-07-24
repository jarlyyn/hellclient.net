namespace Hellclient.World.Types;

public class Info
{

    public required Ring<Line> Lines { get;init; }
    public required Ring<string> History { get;init; }
    public required Ring<Line> Recent { get;init; }
    public Line Prompt = new();
    public int LineCount;
    public int Priority;
    public long LastActive;
    public List<Line> Summary = [];

}