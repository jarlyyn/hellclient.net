namespace Hellclient.Core.Features.States;

using Hellclient.Core.Infras.Components;

public class MessengerContext
{
    public Room Room { get; set; } = new Room();
    public required TitanContext TitanContext { get; set; }

}