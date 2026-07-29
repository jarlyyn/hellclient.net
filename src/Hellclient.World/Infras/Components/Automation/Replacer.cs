using System.Text.RegularExpressions;
using Hellclient.World.Types;

namespace Hellclient.World.Components.Automation;


public class Replacer
{
    public static List<ReplacePair> BuildParamsReplacer(Dictionary<string, string> parameters)
    {
        var result = new List<ReplacePair>();
        result.Add(new ReplacePair("\\\\", "\\"));
        result.Add(new ReplacePair("\\@", "@"));
        var values = parameters.ToList();
        values.Sort((a, b) => b.Key.CompareTo(a.Key));
        foreach (var kv in values)
        {
            result.Add(new ReplacePair($"@{kv.Key}", kv.Value));
        }
        return result;
    }
    public static List<ReplacePair> BuildParamsTriggerReplacer(Dictionary<string, string> parameters)
    {
        var result = new List<ReplacePair>();
        result.Add(new ReplacePair("\\\\", "\\"));
        result.Add(new ReplacePair("\\@", "@"));
        var values = parameters.ToList();
        values.Sort((a, b) => b.Key.CompareTo(a.Key));
        foreach (var kv in values)
        {
            result.Add(new ReplacePair($"@{kv.Key}", kv.Value));
            result.Add(new ReplacePair($"@!{kv.Key}", Regex.Escape(kv.Value)));
        }
        return result;
    }

    public static string Replace(string input, List<ReplacePair> replacers)
    {
        string pattern = string.Join("|", replacers.Select(r => Regex.Escape(r.From)));
        return Regex.Replace(input, pattern, match =>
        {
            return replacers.First(r => r.From == match.Value).To;
        });
    }
}