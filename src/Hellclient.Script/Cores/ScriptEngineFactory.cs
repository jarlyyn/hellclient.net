namespace Hellclient.Script.Cores;

using Hellclient.World.Types;
using Hellclient.World.Infras.Adapters;
using Hellclient.World.Cores;
using Hellclient.Script.Types;

public interface IScriptEngineFactory
{
    public IScriptEngine CreateScriptEngine(IWorld world);
    public string Label();
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
    public string Label()
    {
        return "";
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
    public static List<ScriptType> ListScriptTypes()
    {
        return _factories.ToList().Where(kv => kv.Key != "").Select(kv => new ScriptType { Key = kv.Key, Label = kv.Value.Label() }).ToList();
    }
}