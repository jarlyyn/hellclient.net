using Hellclient.World.Contexts;

namespace Hellclient.World.Shared;


public partial class World : IWorld
{
    public WorldEventBus EventBus { get => Context.EventBus; }
    public WorldContext Context { get; set; } = new WorldContext();
}