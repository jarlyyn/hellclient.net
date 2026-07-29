namespace Hellclient.World.Types;

public class MatchResult
{
    public List<string> List { get; set; } = [];
    public Dictionary<string, string> Named { get; set; } = [];
    public List<ReplacePair> ReplaceList(string name)
    {
        var result = new List<ReplacePair>()
        {
            new("%%","%"),
            new ("%N",name),
            new ("$C",""),
        };
        for (var i = 0; i < List.Count; i++)
        {
            if (i < 10)
            {
                result.Add(new($"%{i}", List[i]));
            }
            else
            {
                result.Add(new($"%<{i}>", List[i]));
            }
        }
        foreach (var kv in Named)
        {
            result.Add(new($"%<{kv.Key}>", kv.Value));
        }
        return result;
    }
}

public interface IMatcher
{
    public MatchResult? Match(string message);
}