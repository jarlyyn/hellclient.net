using Hellclient.World.Types;

namespace Hellclient.World.Components.Automation;

public class MatcherBuilder
{
    public static IMatcher Build(string pattern,bool isregexp,bool ignore_case )
    {
        if (isregexp)
        {
            return new RegexpMatcher(pattern, ignore_case);
        }
        else
        {
            return new StarMatcher(pattern, ignore_case);
        }

    }
}