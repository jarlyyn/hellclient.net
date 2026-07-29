using Hellclient.World.Types;
using System.Text.RegularExpressions;
namespace Hellclient.World.Components.Automation;

public class StarMatcher : IMatcher
{
    public StarMatcher(string pattern, bool ignoreCase)
    {
        string escaped = Regex.Escape(pattern);
        string regexPattern = "^" + escaped.Replace("\\*", ".*") + "$";
        _matcher = new Regex(regexPattern, ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
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
        return r;
    }
}