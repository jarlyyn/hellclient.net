namespace Hellclient.Core.Models;

public class Timer
{
    public const int TimerFlagEnabled = 1;
    public const int TimerFlagAtTime = 2;
    public const int TimerFlagOneShot = 4;
    public const int TimerFlagTimerSpeedWalk = 8;
    public const int TimerFlagTimerNote = 16;
    public const int TimerFlagActiveWhenClosed = 32;
    public const int TimerFlagReplace = 1024;
    public const int TimerFlagTemporary = 16384;
    public string ID { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool Enabled { get; set; } = false;
    public int Hours { get; set; } = 0;
    public int Minute { get; set; } = 0;
    public double Second { get; set; } = 0;
    public string Send { get; set; } = string.Empty;
    public string Script { get; set; } = string.Empty;
    public bool AtTime { get; set; } = false;
    public bool ActionWhenDisconnectd { get; set; } = false;
    public bool Temporary { get; set; } = false;
    public bool OneShot { get; set; } = false;
    public string Group { get; set; } = string.Empty;
    public string Variable { get; set; } = string.Empty;
    public bool OmitFromLog { get; set; } = false;
    public bool OmitFromOutput { get; set; } = false;
    private bool byuser = false;

}