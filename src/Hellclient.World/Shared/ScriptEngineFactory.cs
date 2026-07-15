namespace Hellclient.World.Shared;

using Hellclient.World.Interfaces;
using Hellclient.World.Adapters;

public interface IScriptEngineFactory
{
    public IScriptEngine CreateScriptEngine(IWorld world);
}

public class NopScriptEngineFactory : IScriptEngineFactory
{
    public IScriptEngine CreateScriptEngine(IWorld world)
    {
        return new NopScriptEngine();
    }
}

public static class ScriptEngineFactoryManager
{
    public static IScriptEngineFactory DefaultFactory { get; set; } = new NopScriptEngineFactory();
    private static Dictionary<string, IScriptEngineFactory> _factories = [];
    public static void RegisterFactory(string name, IScriptEngineFactory factory)
    {
        _factories[name] = factory;
    }
    public static IScriptEngine CreateScriptEngine(string name, IWorld world)
    {
        if (_factories.ContainsKey(name))
        {
            return _factories[name].CreateScriptEngine(world);
        }
        else
        {
            return DefaultFactory.CreateScriptEngine(world);
        }
    }
}