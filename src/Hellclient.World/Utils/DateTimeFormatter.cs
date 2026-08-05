namespace Hellclient.World.Utils;

public class DateTimeFormatter
{
    public static string Format(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
}