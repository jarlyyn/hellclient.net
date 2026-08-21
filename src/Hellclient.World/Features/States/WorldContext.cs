using Hellclient.World.Infras.Adapters;
using Hellclient.World.Types;
using Hellclient.World.Infras.Components;
using Convert = Hellclient.World.Infras.Components.Convert;
using Hellclient.World.Components.Automation;
using Hellclient.Infras.Components;

namespace Hellclient.World.States;

public class WorldContext
{
    //游戏上下文
    public void Dispose()
    {

    }
    //异步锁
    public SemaphoreSlim Lock { get; set; } = new SemaphoreSlim(1, 1);
    //游戏ID
    public string ID { get; set; } = string.Empty;
    //事件总线，供上层代码监听
    public WorldEventBus EventBus { get; set; } = new WorldEventBus();
    //脚本引擎
    public IScriptEngine ScriptEngine { get; set; } = new NopScriptEngine();
    //Mud连接
    public required IMudConnection Connection { get; set; }
    //转换器
    public IConvert Convert { get; set; } = new Convert();
    //限流器组件
    public Metronome Metronome { get; set; } = new Metronome();
    //定时队列组件
    public Queue Queue { get; set; } = new Queue();
    //当前游戏各类信息
    public required Info Info { get; set; }
    //当前游戏Hud信息

    public HUD HUD { get; set; } = new HUD();
    //游戏配置
    public WorldConfig Config { get; set; } = new WorldConfig();
    //自动化(触发/别名/定时器)组件
    public Automation Automation { get; set; } = new Automation();
    //脚本数据组件
    public Hellclient.World.Infras.Components.Script Script { get; set; } = new Hellclient.World.Infras.Components.Script();
    //游戏路径信息
    public required WorldPaths Paths { get; init; }
    // 向mud报告的客户端类型信息
    public List<string> TType = [];
    //日志实例
    public required ILogger logger { get; init; }
    // 脚本引擎创建器
    public required Func<string, IScriptEngine> EngineCreator { get; init; }
}