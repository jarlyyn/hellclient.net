using Hellclient.World.Features.WorldServices;

namespace Hellclient.World.Cores;

public interface IWorldManager
{
    public World NewWorld(string id);
}

public class WorldManager : IWorldManager
{
    private IWorldService _service;
    public WorldManager(IWorldService service)
    {
        _service = service;
    }
    public World NewWorld(string id)
    {
        return new World(id, _service);
    }
}