using Hellclient.Core.Types;

namespace Hellclient.Core.Infras.Components;

public class TitanEventBus : IPublisher
{
    public EventHandler<Message>? MsgEvent { get; set; }
    public EventHandler<World.Types.Message>? RequestEvent { get; set; }
    public void Publish(Message message)
    {
        MsgEvent?.Invoke(this, message);
    }

}