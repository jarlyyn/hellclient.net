using Hellclient.World.States;

namespace Hellclient.World.Cores;


public partial class World : IWorld
{
    public World(string id)
    {
        Context.ID = id;
        init();
    }
    public WorldEventBus EventBus { get => Context.EventBus; }
    public WorldContext Context { get; set; } = new WorldContext();
}