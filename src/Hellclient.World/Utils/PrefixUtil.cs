namespace Hellclient.World.Utils;

public class PrefixUtil
{
    public const string PrefixByUser = "u";
    public const string PrefixByScript = "s";

    public static string PrefixedName(string name, bool byuser)
    {
        return $"{(byuser ? PrefixByUser : PrefixByScript)}{name}";
    }
}