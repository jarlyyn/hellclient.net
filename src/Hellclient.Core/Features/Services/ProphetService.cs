using Hellclient.Core.Features.States;
using Hellclient.Core.Types;

namespace Hellclient.Core.Features.Services;

public interface IProphetService
{
    void Enter(ProphetContext ctx, IConnection conn);
}
public class ProphetService : IProphetService
{
    public void Enter(ProphetContext ctx, IConnection conn)
    {
        ctx.Users.Login("user", conn);
    }
}