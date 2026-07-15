using Hellclient.World.Contexts;
namespace Hellclient.World;

public class World
{
    public string ID { get; set; } = string.Empty;
    public WorldContext Context { get; set; } = new WorldContext();
}