namespace Hellclient.World.Types;

public class Colour
{
    public static Dictionary<string, int> Colours { get; } = new()
    {
        ["Black"] = 0x000000,
        ["Red"] = 0x7f0000,
        ["Green"] = 0x009300,
        ["Yellow"] = 0xfc7f00,
        ["Blue"] = 0x00007f,
        ["Magenta"] = 0x9c009c,
        ["Cyan"] = 0x009393,
        ["White"] = 0xd2d2d2,
        ["BrightBlack"] = 0x7f7f7f,
        ["BrightRed"] = 0xff0000,
        ["BrightGreen"] = 0x00fc00,
        ["BrightYellow"] = 0xffff00,
        ["BrightBlue"] = 0x0000fc,
        ["BrightMagenta"] = 0xff00ff,
        ["BrightCyan"] = 0x00ffff,
        ["BrightWhite"] = 0xffffff,
        ["BGBlack"] = 0x000000,
        ["BGRed"] = 0x7f0000,
        ["BGGreen"] = 0x009300,
        ["BGYellow"] = 0xfc7f00,
        ["BGBlue"] = 0x00007f,
        ["BGMagenta"] = 0x9c009c,
        ["BGCyan"] = 0x009393,
        ["BGWhite"] = 0xd2d2d2,
        ["BGBrightBlack"] = 0x7f7f7f,
        ["BGBrightRed"] = 0xff0000,
        ["BGBrightGreen"] = 0x00fc00,
        ["BGBrightYellow"] = 0xffff00,
        ["BGBrightBlue"] = 0x0000fc,
        ["BGBrightMagenta"] = 0xff00ff,
        ["BGBrightCyan"] = 0x00ffff,
        ["BGBrightWhite"] = 0xffffff,
        ["Bright-Black"] = 0x7f7f7f,
        ["Bright-Red"] = 0xff0000,
        ["Bright-Green"] = 0x00fc00,
        ["Bright-Yellow"] = 0xffff00,
        ["Bright-Blue"] = 0x0000fc,
        ["Bright-Magenta"] = 0xff00ff,
        ["Bright-Cyan"] = 0x00ffff,
        ["Bright-White"] = 0xffffff,
    };
    public static Dictionary<string, int> NamedColor = new()
    {
        ["black"] = Colours["Black"],
        ["red"] = Colours["Red"],
        ["green"] = Colours["Green"],
        ["yellow"] = Colours["Yellow"],
        ["blue"] = Colours["Blue"],
        ["magenta"] = Colours["Magenta"],
        ["cyan"] = Colours["Cyan"],
        ["white"] = Colours["White"],

    };
    public static int GetNormalColour(int code)
    {
        switch (code)
        {
            case 1:
                return Colours["Black"];
            case 2:
                return Colours["Red"];
            case 3:
                return Colours["Green"];
            case 4:
                return Colours["Yellow"];
            case 5:
                return Colours["Blue"];
            case 6:
                return Colours["Magenta"];
            case 7:
                return Colours["Cyan"];
            case 8:
                return Colours["White"];
        }
        return -1;
    }
    public static int GetBoldColour(int code)
    {
        switch (code)
        {
            case 1:
                return Colours["BrightBlack"];
            case 2:
                return Colours["BrightRed"];
            case 3:
                return Colours["BrightGreen"];
            case 4:
                return Colours["BrightYellow"];
            case 5:
                return Colours["BrightBlue"];
            case 6:
                return Colours["BrightMagenta"];
            case 7:
                return Colours["BrightCyan"];
            case 8:
                return Colours["BrightWhite"];
        }
        return 0;
    }
}