using Hellclient.Core.Features.Services;
using Hellclient.Core.Features.States;
using Hellclient.Core.Types;

namespace Hellclient.Core.Cores;

public class Prophet
{
    public required ProphetContext Context { private get; init; }

    public IProphetService ProphetService { private get; init; } = new ProphetService();

    public void Enter(IConnection conn)=> ProphetService.Enter(Context, conn);
}