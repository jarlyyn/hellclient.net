using Hellclient.Core.Types;

namespace Hellclient.Core.Infras.Components;

public class TitanEventBus
{
    public EventHandler<Message>? MsgEvent { get; set; }
    public EventHandler<World.Types.Message>? RequestEvent { get; set; }
}