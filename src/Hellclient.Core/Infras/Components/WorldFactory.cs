using Hellclient.World.Cores;
using Hellclient.World.Features.WorldServices;
using Hellclient.World.Types;

namespace Hellclient.Core.Infras.Components;


public interface IWorldFactory
{
    IWorld CreateWorld(string id, WorldPaths paths);
}
public class WorldFactory:IWorldFactory
{
    public IWorldService Service { get; set; }=new WorldService();
    public IWorld CreateWorld(string id,WorldPaths paths)
    {
        return new Hellclient.World.Cores.World(id, Service,paths);
    }
}