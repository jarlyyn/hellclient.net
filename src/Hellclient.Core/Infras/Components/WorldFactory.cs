using Hellclient.World.Cores;
using Hellclient.World.Features.WorldServices;

namespace Hellclient.Core.Infras.Components;


public interface IWorldFactory
{
    IWorld CreateWorld(string id);
}
public class WorldFactory:IWorldFactory
{
    public IWorldService Service { get; set; }=new WorldService();
    public IWorld CreateWorld(string id)
    {
        return new Hellclient.World.Cores.World(id, Service);
    }
}