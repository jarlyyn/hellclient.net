using Hellclient.World.Models;

namespace Hellclient.World.Contexts;

public class WorldContext
{
    public string ID { get; set; } = string.Empty;
    public int MaxRecent { get; set; } = 0;
    public int MaxLines { get; set; } = 0;
    public int MaxHistory { get; set; } = 0;

    public WorldData Data { get; set; } = new WorldData();
    public WorldEventBus EventBus { get; set; } = new WorldEventBus();
}