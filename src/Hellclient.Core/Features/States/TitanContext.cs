using Hellclient.World.Configs;
using Hellclient.Core.Infras.Components;
using Hellclient.Core.Types;
using Hellclient.World.Cores;
using Hellclient.World.Types;

namespace Hellclient.Core.Features.States;

public class TitanContext
{
    public Dictionary<string, IWorld> Worlds = new();
    public TitanEventBus EventBus { get; set; } = new TitanEventBus();
    public HellSwitch HellSwitch { get; set; } = new HellSwitch();
    public required Deployment Deployment { get; init; }
    public string ScriptPath { get; set; } = "";
    public string WorldsPath { get; set; } = "";
    public required ILogger Logger { get; init; }
    public required IWorldFactory WorldFactory { get; init; }

}