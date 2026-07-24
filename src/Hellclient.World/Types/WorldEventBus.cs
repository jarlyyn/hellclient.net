namespace Hellclient.World.Types;

public class WorldEventBus
{
        public EventHandler<Line>? LineEvent { get; set; }
        public EventHandler? CloseEvent { get; set; }

        public EventHandler? DisconnectedEvent { get; set; }
        public EventHandler? ServerCloseEvent { get; set; }
}