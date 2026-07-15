using Hellclient.World.Services;

namespace Hellclient.World.Shared;

public partial class World
{
        public void DoSendToConn(byte[] cmd) => WorldService.Instance.ConnService.Send(Context, cmd);
        public void DoConnectServer() => WorldService.Instance.ConnService.Connect(Context);
        public void DoCloseServer() => WorldService.Instance.ConnService.Close(Context);
        public byte[] GetConnBuffer() => WorldService.Instance.ConnService.GetBuffer(Context);
        public bool GetConnConnected() => WorldService.Instance.ConnService.IsConnected(Context);

}