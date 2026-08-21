using System.Text.Json;
using Hellclient.Core.Features.States;
using Hellclient.Core.Infras.Components;
using Hellclient.Core.Types;

namespace Hellclient.Core.Features.Services;

public interface IMessengerService
{
    public void InstallTo(MessengerContext context);
    public void Enter(MessengerContext context, IConnection conn);
}


public class MessengerService : IMessengerService
{
    public ITitanService TitanService { get; set; } = new TitanService();

    public void InstallTo(MessengerContext context)
    {
        context.TitanContext.EventBus.RequestEvent += (sender, msg) => Publish(context, msg);
    }
    public void Enter(MessengerContext context, IConnection conn)
    {
        conn.OnClose += (sender, e) => Task.Run(() => OnClose(context, conn));
        conn.OnMessage += (sender, e) => Task.Run(() => OnMessage(context, new ConnectionMessage()
        {
            Connection = conn,
            Message = e,
        }));
        OnOpen(context, conn);
    }
    public void OnMessage(MessengerContext context, ConnectionMessage cmsg)
    {
        try
        {
            var msg = JsonSerializer.Deserialize<Hellclient.World.Types.Message>(cmsg.Message, JsonContext.Instance.WorldMessage);
            if (msg == null)
            {
                return;
            }
            switch (msg.Command)
            {
                case Hellclient.World.Types.Message.MessageCommandResponse:
                    TitanService.OnResponse(context.TitanContext, msg);
                    break;
                case Hellclient.World.Types.Message.MessageCommandBatchCommand:
                    TitanService.OnBatchCommandMessage(context.TitanContext, msg);
                    break;
            }
        }
        catch (Exception)
        {
        }
    }
    public void OnOpen(MessengerContext context, IConnection conn)
    {
        context.Room.Join(conn);
    }
    public void OnClose(MessengerContext context, IConnection conn)
    {
        context.Room.Leave(conn);
    }
    public void Publish(MessengerContext context, Hellclient.World.Types.Message msg)
    {
        var data = JsonContext.Serialize(msg);
        context.Room.Broadcast(data);
    }
}