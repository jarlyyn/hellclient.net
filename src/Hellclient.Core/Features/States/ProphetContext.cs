using Hellclient.Core.Configs;
using Hellclient.Core.Infras.Components;

namespace Hellclient.Core.Features.States;

public class ProphetContext
{
    public required TitanContext TitanContext { get; set; }
    public string Current="";
    public Users Users { get; set; } = new Users();
    public required Deployment Deployment { get; init; }

    public Rooms Rooms { get; set; } = new Rooms();
    public Adapter Adapter { get; set; } = new Adapter();
    public Handlers Handlers { get; set; } = new Handlers();
}