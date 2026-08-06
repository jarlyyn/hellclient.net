using Hellclient.World.Configs;
using Hellclient.World.Features.WorldServices;
using Hellclient.World.Infras.Components;
using Hellclient.World.States;
using Hellclient.World.Types;

namespace Hellclient.World.Cores;


public partial class World : IWorld
{
    public World(string id, IWorldService service, WorldPaths paths)
    {
        Context = new WorldContext()
        {
            ID = id,
            Paths = paths,
            Info = new Info()
            {
                Lines = new Ring<Line>(AppConfig.System.MaxHistory),
                History = new Ring<string>(AppConfig.System.MaxHistory),
                Recent = new Ring<Line>(AppConfig.System.MaxHistory),
            }
        };
        Service = service;
        Service.InstallTo(Context);

    }
    public void Dispose()
    {
        Task.Run(async () => await DoCloseServer());
    }
    private IWorldService Service { get; init; }
    public WorldEventBus EventBus { get => Context.EventBus; }
    public WorldContext Context { get; init; }
}