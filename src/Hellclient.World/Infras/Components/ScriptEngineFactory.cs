namespace Hellclient.World.Cores;

using Hellclient.World.Types;
using Hellclient.World.Infras.Adapters;

public interface IScriptEngineFactory
{
    public IScriptEngine CreateScriptEngine(IWorld world);
    public void NewScript(string ID);
}

public class NopScriptEngineFactory : IScriptEngineFactory
{
    public IScriptEngine CreateScriptEngine(IWorld world)
    {
        return new NopScriptEngine();
    }
    public void NewScript(string ID)
    {
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
    public static bool HasScriptEngine(string name)
    {
        return _factories.ContainsKey(name);
    }
    public static void NewScript(string name, string ID)
    {
        if (_factories.ContainsKey(name))
        {
            _factories[name].NewScript(ID);
        }
        else
        {
            DefaultFactory.NewScript(ID);
        }
    }
}