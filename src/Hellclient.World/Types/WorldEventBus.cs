namespace Hellclient.World.Types;

public class WorldEventBus
{
        public EventHandler<Line>? LineEvent { get; set; }
        public EventHandler<Line>? PromptEvent { get; set; }
        public EventHandler? CloseEvent { get; set; }

        public EventHandler? DisconnectedEvent { get; set; }
        public EventHandler? ServerCloseEvent { get; set; }
        public EventHandler<List<Line>>? LinesEvent { get; set; }
        public EventHandler<List<string>>? HistoriesEvent { get; set; }
        public EventHandler<ClientInfo>? ClientInfoEvent { get; set; }

}