namespace Hellclient.Core.Types;

public class TitanEventBus
{
    public EventHandler<Message>? MsgEvent { get; set; }
    public EventHandler<World.Types.Message>? RequestEvent { get; set; }
}