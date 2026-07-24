using Hellclient.World.Infras.Adapters;
using Hellclient.World.Types;
using Hellclient.World.Infras.Components;
using Convert = Hellclient.World.Infras.Components.Convert;

namespace Hellclient.World.States;

public class WorldContext
{
    public string ID { get; set; } = string.Empty;
    public WorldData Data { get; set; } = new WorldData();
    public WorldEventBus EventBus { get; set; } = new WorldEventBus();
    public IScriptEngine ScriptEngine { get; set; } = new NopScriptEngine();
    public IMudConnection Connection { get; set; } = new Telnet();
    public IConvert Convert { get; set; } = new Convert();
    public Metronome Metronome { get; set; } = new Metronome();

}