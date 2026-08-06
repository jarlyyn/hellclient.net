using System.Text.RegularExpressions;
using Hellclient.World.Components.Automation;

namespace Hellclient.World.Utils;

public class IDRegexp
{
    private static Regex regex = new Regex(@"^[0-9a-zA-Z\-\_\@\.\[\]\(\)\+]*$");
    public static bool MatchString(string target)
    {
        return regex.IsMatch(target);
    }
}