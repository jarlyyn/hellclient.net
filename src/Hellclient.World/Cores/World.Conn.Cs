using Hellclient.World.Features.Services;

namespace Hellclient.World.Cores;

public partial class World
{
        public void DoSendToConn(byte[] cmd) => Service.ConnService.Send(Context, cmd);
        public void DoConnectServer() => Service.ConnService.Connect(Context);
        public void DoCloseServer() => Service.ConnService.Close(Context);
        public byte[] GetConnBuffer() => Service.ConnService.GetBuffer(Context);
        public bool GetConnConnected() => Service.ConnService.IsConnected(Context);

}