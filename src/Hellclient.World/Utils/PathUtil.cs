namespace Hellclient.World.Utils;

public class PathUtil
{
    public static string GetRootPath()
    {
        return System.IO.Directory.GetParent(System.IO.Path.GetDirectoryName(Environment.ProcessPath) ?? string.Empty)?.FullName ?? string.Empty;
    }
}