using Hellclient.World.Configs;
using Hellclient.World.Features.WorldServices;
using Hellclient.World.States;
using Hellclient.World.Types;

namespace Hellclient.World.Cores;


public partial class World : IWorld
{
    public World(string id, IWorldService service)
    {
        Context.ID = id;
        Service = service;
        Service.InstallTo(Context);
        init();

    }
    private IWorldService Service { get; init; }
    public WorldEventBus EventBus { get => Context.EventBus; }
    public WorldContext Context { get; set; } = new WorldContext()
    {
        Info = new Info()
        {
            Lines = new Ring<Line>(AppConfig.System.MaxHistory),
            History = new Ring<string>(AppConfig.System.MaxHistory),
            Recent = new Ring<Line>(AppConfig.System.MaxHistory),
        }
    };
}