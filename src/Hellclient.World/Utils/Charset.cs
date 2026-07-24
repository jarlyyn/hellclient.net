using System.Text;

namespace Hellclient.World.Utils;

public class CharsetUtil
{
    public static string UTF8 { get; }= "utf-8";
    public static string GBK { get; } = "gbk";
    public static void InstallEncodingProvider()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }
    public static byte[] FromUtf8(string charset, string data)
    {
        if (data == null)
        {
            return Array.Empty<byte>();
        }
        switch (charset)
        {
            case "gbk":
            case "cp936":
            case "windows-936":
                return Encoding.GetEncoding(charset).GetBytes(data);
            case "utf-8":
                return Encoding.UTF8.GetBytes(data);
            default:
                throw new NotSupportedException($"Charset {charset} is not supported.");
        }
    }
    public static string ToUtf8(string charset, byte[] data)
    {
        switch (charset)
        {
            case "gbk":
            case "cp936":
            case "windows-936":
                return Encoding.GetEncoding(charset).GetString(data);
            case "utf-8":
                return Encoding.UTF8.GetString(data);
            default:
                throw new NotSupportedException($"Charset {charset} is not supported.");
        }
    }
}