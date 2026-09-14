using Hellclient.World.Types;
using PCRE;
namespace Hellclient.World.Components.Automation;

public class RegexpMatcher : IMatcher
{
    public RegexpMatcher(string pattern, bool ignoreCase)
    {
        PcreOptions options = PcreOptions.None;
        if (ignoreCase)
        {
            options |= PcreOptions.IgnoreCase;
        }
        this._matcher = new PcreRegex(pattern, options);
    }
    private PcreRegex _matcher { get; init; }
    public MatchResult? Match(string message)
    {
        var result = _matcher.Match(message);
        if (!result.Success)
        {
            return null;
        }
        var r = new MatchResult();
        result.Groups.ToList().ForEach(g =>
        {
            r.List.Add(g.Value);
        });
        _matcher.PatternInfo.GroupNames.ToList().ForEach(name =>
        {
            r.Named.Add(name, result.Groups[name].Value);
        });
        return r;
    }
}