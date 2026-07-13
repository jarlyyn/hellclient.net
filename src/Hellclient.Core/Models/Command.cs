namespace Hellclient.Core.Models;

public class Command
{
    public string Message { get; set; } = string.Empty;
    public bool Echo { get; set; } = false;
    public bool Queue { get; set; } = false;
    public bool Log { get; set; } = false;
    public bool History { get; set; } = false;
    public bool Locked { get; set; } = false;
    public string Creator { get; set; } = string.Empty;
    public string CreatorType { get; set; } = string.Empty;

}