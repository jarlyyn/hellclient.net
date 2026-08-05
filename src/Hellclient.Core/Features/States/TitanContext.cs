using Hellclient.Core.Configs;
using Hellclient.Core.Infras.Components;
using Hellclient.Core.Types;
using Hellclient.World.Cores;

namespace Hellclient.Core.Features.States;

public class TitanContext
{
    public Dictionary<string, IWorld> Worlds = new();
    public TitanEventBus EventBus { get; set; } = new TitanEventBus();
    public HellSwitch HellSwitch { get; set; } = new HellSwitch();
    public required Deployment Deployment { get; init; }
}