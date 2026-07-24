namespace Hellclient.World.Types;



public class Metronome
{
    public static TimeSpan DefaultCheckInterval = TimeSpan.FromMilliseconds(50);
    public static TimeSpan DefaultInterval = TimeSpan.FromMilliseconds(1000);

    public static int DefaultBeats = 10;
    public PeriodicTimer? ticker { get; set; }
    public TimeSpan Tick { get; set; }
    public int Beats { get; set; }

    public List<List<Command>> Queue { get; set; } = new();

    public List<DateTime> Sent { get; set; } = new();
    public TimeSpan Interval { get; set; }
}