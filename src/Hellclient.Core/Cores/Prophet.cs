using Hellclient.Core.Features.Services;
using Hellclient.Core.Features.States;
using Hellclient.Core.Infras.Components;
using Hellclient.Core.Types;
using Hellclient.World.Infras.Components;

namespace Hellclient.Core.Cores;

public partial class Prophet
{
    public void Init()
    {
        initAdapter();
        initHandlers();
        Context.TitanContext.EventBus.MsgEvent += (sender, msg) => ProphetService.Publish(Context, msg);
    }

    public required ProphetContext Context { private get; init; }
    public Rooms Rooms { get => Context.Rooms; }

    public IProphetService ProphetService { private get; init; } = new ProphetService();

    public void Enter(IConnection conn) => ProphetService.Enter(Context, conn);

    public void SendToUser(byte[] data) => ProphetService.SendToUser(Context, data);
    public void OnMessage(ConnectionMessage msg) => Context.Handlers.Exec(msg);
    public void OnOpen(IConnection conn) => ProphetService.OnOpen(Context, conn);
    public void OnClose(IConnection conn) => ProphetService.OnClose(Context, conn);
    public string GetCurrent() => ProphetService.GetCurrent(Context);
    public void SetAuth(string username, string password) => ProphetService.SetAuth(Context, username, password);
    public bool CheckAuth(string username, string password) => ProphetService.CheckAuth(Context, username, password);
}