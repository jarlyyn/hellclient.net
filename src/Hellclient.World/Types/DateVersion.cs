namespace Hellclient.World.Types;

public class DateVersion
{
    public int Major { get; set; } = 0;
    public int Year { get; set; } = 0;
    public int Month { get; set; } = 0;
    public int Day { get; set; } = 0;
    public int Patch { get; set; } = 0;
    public string Build { get; set; } = string.Empty;
    public string MajorVersionCode()
    {
        return $"{Major}";
    }
    public Int64 MajorVersionWeight()
    {
        return Major;
    }
    public string FullVersionCode()
    {
        var month = Month.ToString().PadLeft(2, '0');
        var day = Day.ToString().PadLeft(2, '0');
        var patch = Patch == 0 ? "" : $".{Patch}";
        var build = string.IsNullOrEmpty(Build) ? "" : $" {Build}";
        return $"{Major}.{Year}-{month}-{day}.{patch}{build}";
    }
}