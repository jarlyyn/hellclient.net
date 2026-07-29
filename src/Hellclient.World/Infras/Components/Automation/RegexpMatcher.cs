using Hellclient.World.Types;
using System.Text.RegularExpressions;
namespace Hellclient.World.Components.Automation;

public class RegexpMatcher : IMatcher
{
    public RegexpMatcher(string pattern, bool ignoreCase)
    {
        RegexOptions options = RegexOptions.None;
        if (ignoreCase)
        {
            options |= RegexOptions.IgnoreCase;
        }
        this._matcher = new Regex(pattern, options);
    }
    private Regex _matcher { get; init; }
    public MatchResult? Match(string message)
    {
        var result = _matcher.Match(message);
        if (!result.Success)
        {
            return null;
        }
        var r = new MatchResult();
        r.List = result.Groups.Cast<Group>().Select(g => g.Value).ToList();
        r.Named = result.Groups.Cast<Group>().Where(g => !string.IsNullOrEmpty(g.Name)).ToDictionary(g => g.Name, g => g.Value);
        return r;
    }
}