using Hellclient.World.Types;
using Path = System.IO.Path;

namespace Hellclient.World.Helpers;

public class AuthorizeHelper
{
    private static bool MatchDomain(string pattern, string domain)
    {
        if (pattern == "" || domain == "")
        {
            return false;

        }
        if (pattern[0] == '.')
        {
            var l = domain.Split(new[] { '.' }, 2);
            return l.Length == 2 && l[1] == domain.Substring(1);

        }
        else if (pattern[0] == '*')
        {
            return domain.EndsWith(pattern.Substring(1));
        }
        return pattern == domain;

    }
    public static string CleanInsidePath(string _base, string newpath)
    {
        var path = Path.GetFullPath(_base);
        var cleanpath = CleanPath(_base, newpath);
        if (!cleanpath.StartsWith(path))
        {
            return "";
        }
        return cleanpath;
    }
    public static string CleanPath(string _base, string newpath)
    {
        if (!Path.IsPathRooted(newpath))
        {
            var basePath = Path.GetFullPath(_base);
            newpath = Path.Combine(basePath, newpath);
        }

        return Path.GetFullPath(newpath);
    }
    public static bool AuthorizeDomain(Trusted t, string domain)
    {
        for (int i = 0; i < t.Domains.Count; i++)
        {
            if (MatchDomain(t.Domains[i], domain))
            {
                return true;
            }
        }
        return false;
    }
    public static bool AuthorizePath(Trusted t, string path)
    {
        path = Path.GetFullPath(path);
        for (int i = 0; i < t.Paths.Count; i++)
        {
            if (t.Paths[i] != "" && path.StartsWith(t.Paths[i]))
            {
                return true;
            }
        }
        return false;
    }
}