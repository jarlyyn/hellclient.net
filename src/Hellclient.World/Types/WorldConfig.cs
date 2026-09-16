namespace Hellclient.World.Types;

public class WorldConfig
{
    public WorldData Data { get; set; } = WorldData.New();
    public long ReadyAt { get; set; } = 0;
    public int Position { get; set; } = 0;
}