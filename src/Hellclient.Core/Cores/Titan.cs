using Hellclient.Core.Features.States;
using Hellclient.World.Cores;

namespace Hellclient.Core.Cores;

//World管理类，用来管理现有所有的游戏
public class Titan
{
    public IWorldFactory WorldFactory { get; set; }=new WorldFactory();
    private IWorld? _find(TitanContext context, string id)
    {
        return context.Worlds.TryGetValue(id, out var world) ? world : null;
    }
    public IWorld? World(TitanContext context, string id)
    {
        return _find(context, id);
    }
    public IWorld? NewWorld(TitanContext context, string id)
    {
        var world = _find(context, id);
        if (world != null)
        {
            return world;
        }
        world = WorldFactory.CreateWorld(id);
        context.Worlds[id] = world;
        return world;
    }
}