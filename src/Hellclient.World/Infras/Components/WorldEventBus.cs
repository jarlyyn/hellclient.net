using Hellclient.World.Types;

namespace Hellclient.World.Infras.Components;

public class WorldEventBus
{
        public EventHandler<Line>? LineEvent { get; set; }
        public EventHandler<Line>? PromptEvent { get; set; }
        public EventHandler? CloseEvent { get; set; }
        public EventHandler? ConnectedEvent { get; set; }
        public EventHandler? DisconnectedEvent { get; set; }
        public EventHandler? ServerCloseEvent { get; set; }
        public EventHandler<List<Line>>? LinesEvent { get; set; }
        public EventHandler<List<string>>? HistoriesEvent { get; set; }
        public EventHandler<ClientInfo>? ClientInfoEvent { get; set; }
        public EventHandler? QueueDelayUpdatedEvent { get; set; }
        public EventHandler<List<Line>>? HUDContentEvent { get; set; }
        public EventHandler<DiffLines>? HUDUpdateEvent { get; set; }
        public EventHandler? ReadyEvent { get; set; }
        public EventHandler<string>? StatusEvent { get; set; }
        public EventHandler<Message>? RequestEvent { get; set; }
        public EventHandler<Broadcast>? BroadcastEvent { get; set; }
        public EventHandler<Authorization>? RequestPermissionsEvent { get; set; }
        public EventHandler<Authorization>? RequestTrustDomainsEvent { get; set; }
        public EventHandler<object>? ScriptMessageEvent { get; set; }
}