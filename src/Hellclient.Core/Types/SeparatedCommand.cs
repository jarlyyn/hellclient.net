using System.Text;

namespace Hellclient.Core.Types;

public class SeparatedCommand
{
    public static readonly byte[] SeparatorDefault = new byte[] { 32 };
    // CommandType command type
    public string CommandType { get; set; } = "";
    // CommandData command data
    public byte[] CommandData { get; set; } = [];
    //Separator  data separator
    public byte[] Separator { get; set; } = SeparatorDefault;
    public string Type()
    {
        return CommandType;
    }
    public byte[] Data()
    {
        return CommandData;
    }
    public byte[] Encode()
    {
        return Encoding.UTF8.GetBytes(CommandType).Concat(Separator).Concat(CommandData).ToArray();
    }
    public void Decode(byte[] data)
    {
        if (data.Length == 0||Separator.Length == 0)
        {
            return;
        }
        var sepIndex = data.IndexOf(Separator);
        if (sepIndex == -1)
        {
            CommandType = Encoding.UTF8.GetString(data);
            CommandData = Array.Empty<byte>();
            return;
        }
        CommandType = Encoding.UTF8.GetString(data[..sepIndex]);
        CommandData = data[(sepIndex + Separator.Length)..];
    }
}
