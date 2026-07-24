namespace Hellclient.World.Types;

public class Command
{
    public static Command Create(string message) => new()
    {
        Message = message,
        Echo = true,
    };
    public Command Clone()
    {
        return new Command
        {
            Message = this.Message,
            Echo = this.Echo,
            Queue = this.Queue,
            Log = this.Log,
            History = this.History,
            Locked = this.Locked,
            Creator = this.Creator,
            CreatorType = this.CreatorType
        };
    }
    public Command[] Split(string sep)
    {
        return [.. Message.Split(sep).Select((m) =>
        {
            var cmd = this.Clone();
            cmd.Message = m;
            return cmd;
        })];
    }
    public string Message { get; set; } = string.Empty;
    public bool Echo { get; set; } = false;
    public bool Queue { get; set; } = false;
    public bool Log { get; set; } = false;
    public bool History { get; set; } = false;
    public bool Locked { get; set; } = false;
    public string Creator { get; set; } = string.Empty;
    public string CreatorType { get; set; } = string.Empty;

}