using Hellclient.World.Features.WorldServices;

namespace Hellclient.World.Cores;

public class Core
{
    public static Core Instance { get; set; } = BuildDefault();
    public required IWorldManager WorldManager { get; init; }
    public static Core BuildDefault()
    {
        var core = new Core()
        {
            WorldManager = new WorldManager(new WorldService())
        };
        return core;
    }
}