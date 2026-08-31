using Hellclient.World.Types;

namespace Hellclient.World.Configs;

public class AppVersion
{
    public static DateVersion Version { get; set; } = new DateVersion()
    {
        Major = 2,
        Year = 2026,
        Month = 8,
        Day = 31,
        Patch = 0,
        Build = ""
    };
    public static DateVersion APIVersion { get; set; } = new DateVersion()
    {
        Major = 2,
        Year = 2026,
        Month = 8,
        Day = 31,
        Patch = 0,
        Build = ""
    };
}