using Hellclient.World.Features.Services;
using Hellclient.World.States;

namespace Hellclient.World.Cores;


public partial class World : IWorld
{
    public World(string id)
    {
        Context.ID = id;
        init();
    }

    private IWorldService Service{get;init;}= new WorldService();
    public WorldEventBus EventBus { get => Context.EventBus; }
    public WorldContext Context { get; set; } = new WorldContext();
}