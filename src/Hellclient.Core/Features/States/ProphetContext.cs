using Hellclient.World.Configs;
using Hellclient.Core.Infras.Components;
using Hellclient.Core.Types;

namespace Hellclient.Core.Features.States;

public class ProphetContext
{
    public required TitanContext TitanContext { get; set; }
    public volatile string Current = "";
    public Users Users { get; set; } = new Users();
    public required Deployment Deployment { get; init; }

    public Rooms Rooms { get; set; } = new Rooms();
    public Adapter Adapter { get; set; } = new Adapter();
    public Handlers Handlers { get; set; } = new Handlers();
    public volatile UserPassword UserPassword = new UserPassword();
}