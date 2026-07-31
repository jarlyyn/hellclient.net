using Hellclient.World.Cores;

namespace Hellclient.Core.Features.States;

public class TitanContext
{
    public Dictionary<string, IWorld> Worlds = new();
}