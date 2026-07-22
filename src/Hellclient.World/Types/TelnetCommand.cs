namespace Hellclient.World.Types;

public class TelnetCommand(byte command, byte[] data)
{
    public const byte CmdIAC = 255;
    public const byte CmdDo = 253;
    public const byte CmdDont = 254;
    public const byte CmdWill = 251;
    public const byte CmdWont = 252;
    public const byte CmdNOP = 241;
    public const byte CmdDataMark = 242;
    public const byte CmdBreak = 243;
    public const byte CmdInterruptProcess = 244;
    public const byte CmdAbortOutput = 245;
    public const byte CmdAreYouThere = 246;
    public const byte CmdEraseCharacter = 247;
    public const byte CmdEraseLine = 248;
    public const byte CmdGoAhead = 249;
    public const byte CmdSubnegotiation = 250;
    public const byte CmdEndSubnegotiation = 240;
    public const byte CmdGMCP = 201;
    public static TelnetCommand Do(byte option) => new TelnetCommand(CmdDo, [option]);
    public static TelnetCommand Dont(byte option) => new TelnetCommand(CmdDont, [option]);
    public static TelnetCommand Will(byte option) => new TelnetCommand(CmdWill, [option]);
    public static TelnetCommand Wont(byte option) => new TelnetCommand(CmdWont, [option]);
    public static byte[] Subnegotiation(byte option, byte[] data)
    {
        var result = new List<byte> { CmdSubnegotiation, option };
        result.AddRange(data);
        result.Add(CmdEndSubnegotiation);
        return result.ToArray();
    }
    public static byte[] GMCP(byte[] data)=> Subnegotiation(CmdGMCP, data);
   
    public byte Command { get; init; } = command;
    public byte[] Data { get; init; } = data;
    public byte[] ToByteArray()
    {
        var result = new List<byte> { CmdIAC, Command };
        if (Data != null)
        {
            result.AddRange(Data);
        }
        return result.ToArray();
    }

}
