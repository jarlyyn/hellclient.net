using Hellclient.Core.Configs;
using Hellclient.Core.Infras.Components;

namespace Hellclient.Core.Features.States;

public class ProphetContext
{
    public Users Users { get; set; } = new Users();
    public required Deployment Deployment { get; init; }

}