namespace Hellclient.World.Presets;

using Hellclient.World.Models;
public class AppVersion
{
    public static DateVersion Version { get; set; } = new DateVersion()
    {
        Major = 1,
        Year = 2026,
        Month = 5,
        Day = 14,
        Patch = 0,
        Build = ""
    };
    public static DateVersion APIVersion { get; set; } = new DateVersion()
    {
        Major = 1,
        Year = 2026,
        Month = 5,
        Day = 14,
        Patch = 0,
        Build = ""
    };
}