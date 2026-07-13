namespace Hellclient.Core.Models;

public class Message
{
    public string World { get; set; } = string.Empty;
    public string ID { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public int Command { get; set; } = 0;

}