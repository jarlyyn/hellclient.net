using System.Runtime.InteropServices;
using Hellclient.World.Infras.Adapters;

namespace Hellclient.World.Types;

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
    public int Hour { get; set; } = 0;
    public int Minute { get; set; } = 0;
    public double Second { get; set; } = 0;
    public string Send { get; set; } = string.Empty;
    public string Script { get; set; } = string.Empty;
    public bool AtTime { get; set; } = false;
    public int SendTo { get; set; } = 0;
    public bool ActionWhenDisconnectd { get; set; } = false;
    public bool Temporary { get; set; } = false;
    public bool OneShot { get; set; } = false;
    public string Group { get; set; } = string.Empty;
    public string Variable { get; set; } = string.Empty;
    public bool OmitFromLog { get; set; } = false;
    public bool OmitFromOutput { get; set; } = false;
    private bool byuser = false;

    public bool ByUser()
    {
        return byuser;
    }
    public void SetByUser(bool value)
    {
        byuser = value;
    }
    public string PrefixedName()
    {
        return $"{(byuser ? Prefix.PrefixByUser : Prefix.PrefixByScript)}{Name}";
    }
    public TimeSpan GetDuration()
    {
        if (AtTime)
        {
            var now = DateTime.Now;
            var target = new DateTime(now.Year, now.Month, now.Day, Hour, Minute, (int)Second);
            if (target < now)
            {
                target = target.AddDays(1);
            }
            return target - now;
        }
        var d = new TimeSpan(Hour, Minute, (int)Second);
        if (d <= TimeSpan.Zero)
        {
            d = new TimeSpan(0, 0, 1);
        }
        return d;
    }
    public static Timer CreateComInterfaceFlags()
    {
        return new Timer()
        {
            ID = SimpleID.Instance.GenerateID(),
        };
    }
    public int CompareTo(Timer other)
    {
        return ID.CompareTo(other.ID);
    }

}