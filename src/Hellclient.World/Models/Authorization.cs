namespace Hellclient.World.Models;

public class Authorization
{
    public string World { get; set; } = string.Empty;
    public string Script { get; set; } = string.Empty;
    public List<string> Items = [];
    public string Reason { get; set; } = string.Empty;
}

