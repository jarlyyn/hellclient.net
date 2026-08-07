namespace Hellclient.Helpers;

public class ConfigHelper
{
    public static string ConvertListenUrl(string u)
    {
        var data=u.Split(':',2);
        return $"http://{(data[0]==""?"*":data[0])}:{data[1]}";
    }
}


