using Hellclient.World.Cores;
using Hellclient.Script.Cores;
using Hellclient.World.Features.WorldServices;
using Hellclient.World.Types;

namespace Hellclient.Core.Infras.Components;


public interface IWorldFactory
{
    IWorld CreateWorld(string id, WorldPaths paths);
}
public class WorldFactory : IWorldFactory
{
    public WorldFactory(ILogger logger)
    {
        this.logger = logger;
    }
    public ILogger logger { get; init; }

    public IWorldService Service { get; set; } = new WorldService();
    public Func<string, IScriptEngine> ScriptEngineCreatorFactory(IWorld world)
    {
        return (type) => ScriptEngineFactoryManager.CreateScriptEngine(type, world);
    }
    public IWorld CreateWorld(string id, WorldPaths paths)
    {
        return new Hellclient.World.Cores.World(id, Service, paths, logger, ScriptEngineCreatorFactory);
    }
}