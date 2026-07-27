using Hellclient.World.Configs;

namespace Hellclient.World.Cores;

public partial class World
{
	public SemaphoreSlim Lock { get => Context.Lock; }
	public string ID { get => Context.ID; }
	public int GetMaxHistory() => AppConfig.System.MaxHistory;
	public int GetMaxLines() => AppConfig.System.MaxLines;
	public int GetMaxRecent() => AppConfig.System.MaxRecent;
}