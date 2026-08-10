using System.Text;

namespace Hellclient.World.Utils;

public class CharsetUtil
{
    public static string UTF8 { get; } = "utf-8";
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
            case "GBK":
            case "cp936":
            case "CP936":
            case "GB18030":
            case "gb18030":
            case "windows-936":
            case "WINDOWS-936":
                return Encoding.GetEncoding("GB18030").GetBytes(data);
            case "utf8":
            case "UTF8":
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
            case "GBK":
            case "cp936":
            case "CP936":
            case "GB18030":
            case "gb18030":
            case "windows-936":
            case "WINDOWS-936":
                return Encoding.GetEncoding("GB18030").GetString(data);
            case "utf8":
            case "UTF8":
                return Encoding.UTF8.GetString(data);
            default:
                throw new NotSupportedException($"Charset {charset} is not supported.");
        }
    }
}