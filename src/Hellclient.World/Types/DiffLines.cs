namespace Hellclient.World.Types;
public class DiffLines
{
    public DiffLines(int start, List<Line> content)
    {
        Start = start;
        Content = content;
    }
    public int Start { get; init; }
    public List<Line> Content { get; init; }
}
