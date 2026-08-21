using Hellclient.Core.Features.Services;
using Hellclient.Core.Features.States;
using Hellclient.Core.Types;

namespace Hellclient.Core.Cores;

//消息通道类，实现具体的Websockter实现
public class Messenger
{
    public required MessengerContext Context { get; init; }
    public IMessengerService MessengerService { get; init; } = new MessengerService();
    public void init()
    {
        MessengerService.InstallTo(Context);
    }
        public void Enter(IConnection conn) => MessengerService.Enter(Context, conn);

}