namespace Hellclient.World.Types;

public class PlainOptions
{
    public string Home { get; set; } = "";
    public string ModPath { get; set; } = "";

    public string Location { get; set; } = "";
    public Dictionary<string, string> Params { get; set; } = new Dictionary<string, string>();

    public Trusted Trusted { get; set; } = new();
    public List<string> Permissions { get; set; } = new();
}