using Hellclient.World.Types;
using Microlithix.Text.Ansi;
using Microlithix.Text.Ansi.Element;

namespace Hellclient.World.Helpers;

public static class AnsiHelpers
{
    private static readonly AnsiStringParser parser = new();
    public static Line? Parse(string input)
    {
        var result = new Line();
        if (string.IsNullOrEmpty(input))
        {
            return result;
        }
        List<IAnsiStringParserElement> elements;
        try
        {
            elements = parser.Parse(input);
        }
        catch (Exception)
        {
            return null;
        }

        var current = new Word();
        foreach (var element in elements)
        {
            switch (element)
            {
                case AnsiPrintableString text:
                    current.Text = text.Text;
                    result.Words.Add(current);
                    current = new Word();
                    break;
                case AnsiControlString control:
                    break;
                case AnsiControlSequence cs:
                    switch (cs.Function)
                    {
                        case "m":
                            applyFunctionM(current, result, cs);
                            break;
                        case "K":
                            applyFunctionK(current, result, cs);
                            break;
                        case "J":
                            applyFunctionJ(current, result, cs);
                            break;
                        case "D":
                            applyFunctionD(current, result, cs);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        return result;
    }
    private static void applyFunctionD(Word w, Line line, AnsiControlSequence cs)
    {
        foreach (var param in cs.Parameters)
        {
            line.RemoveTail(param.Value);
        }
    }
    private static void applyFunctionM(Word w, Line line, AnsiControlSequence cs)
    {
        foreach (var param in cs.Parameters)
        {
            switch (param.Value)
            {
                case 0:
                    w.Color = "";
                    w.Background = "";
                    w.Bold = false;
                    w.Inverse = false;
                    w.Underlined = false;
                    w.Blinking = false;
                    break;
                case 1:
                    w.Bold = true;
                    break;
                case 2:
                    w.Bold = false;
                    break;
                case 4:
                    w.Underlined = true;
                    break;
                case 5:
                    w.Blinking = true;
                    break;
                case 7:
                    w.Inverse = true;
                    break;
                case 24:
                    w.Underlined = false;
                    break;
                case 25:
                    w.Blinking = false;
                    break;
                case 27:
                    w.Inverse = false;
                    break;
                case 30:
                    w.Color = "Black";
                    break;
                case 31:
                    w.Color = "Red";
                    break;
                case 32:
                    w.Color = "Green";
                    break;
                case 33:
                    w.Color = "Yellow";
                    break;
                case 34:
                    w.Color = "Blue";
                    break;
                case 35:
                    w.Color = "Magenta";
                    break;
                case 36:
                    w.Color = "Cyan";
                    break;
                case 37:
                    w.Color = "White";
                    break;
                case 40:
                    w.Background = "Black";
                    break;
                case 41:
                    w.Background = "Red";
                    break;
                case 42:
                    w.Background = "Green";
                    break;
                case 43:
                    w.Background = "Yellow";
                    break;
                case 44:
                    w.Background = "Blue";
                    break;
                case 45:
                    w.Background = "Magenta";
                    break;
                case 46:
                    w.Background = "Cyan";
                    break;
                case 47:
                    w.Background = "White";
                    break;
                case 90:
                    w.Color = "Bright-Black";
                    break;
                case 91:
                    w.Color = "Bright-Red";
                    break;
                case 92:
                    w.Color = "Bright-Green";
                    break;
                case 93:
                    w.Color = "Bright-Yellow";
                    break;
                case 94:
                    w.Color = "Bright-Blue";
                    break;
                case 95:
                    w.Color = "Bright-Magenta";
                    break;
                case 96:
                    w.Color = "Bright-Cyan";
                    break;
                case 97:
                    w.Color = "Bright-White";
                    break;
                case 100:
                    w.Background = "Bright-Black";
                    break;
                case 101:
                    w.Background = "Bright-Red";
                    break;
                case 102:
                    w.Background = "Bright-Green";
                    break;
                case 103:
                    w.Background = "Bright-Yellow";
                    break;
                case 104:
                    w.Background = "Bright-Blue";
                    break;
                case 105:
                    w.Background = "Bright-Magenta";
                    break;
                case 106:
                    w.Background = "Bright-Cyan";
                    break;
                case 107:
                    w.Background = "Bright-White";
                    break;
                case 256:
                    line.Words.Clear();
                    break;
                default:
                    break;
            }
        }
    }
    private static void applyFunctionK(Word w, Line line, AnsiControlSequence cs)
    {
        foreach (var param in cs.Parameters)
        {
            switch (param.Value)
            {
                case 1:
                case 2:
                    line.Words.Clear();
                    break;
                default:
                    Console.WriteLine($"Unknown ANSI control sequence function K parameter: {param.Value}");
                    break;
            }
        }
    }
    private static void applyFunctionJ(Word w, Line line, AnsiControlSequence cs)
    {
        foreach (var param in cs.Parameters)
        {
            switch (param.Value)
            {
                case 1:
                case 2:
                    line.Words.Clear();
                    break;
            }
        }
    }

}