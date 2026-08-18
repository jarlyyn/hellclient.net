using Hellclient.World.Types;

namespace Hellclient.World.Cores;

public partial class World
{
        public void DoSendToConn(byte[] cmd) => Service.ConnService.Send(Context, cmd);
        public async Task DoConnectServer() => await Service.ConnService.Connect(Context);
        public async Task DoCloseServer() => await Service.ConnService.Close(Context);
        public bool GetConnConnected() => Service.ConnService.IsConnected(Context);
        public async Task DoSend(Command cmd) => await Service.ConvertService.DoSend(Context, cmd);
        public void DoPrint(string msg) => Service.ConvertService.DoPrint(Context, msg);
        public void DoPrintSystem(string msg) => Service.ConvertService.DoPrintSystem(Context, msg);
        public void DoPrintGlobalBroadcastIn(string msg)=> Service.ConvertService.DoPrintGlobalBroadcastIn(Context, msg);
        public void DoPrintLocalBroadcastOut(string msg)=> Service.ConvertService.DoPrintLocalBroadcastOut(Context, msg);
        public void DoPrintGlobalBroadcastOut(string msg)=> Service.ConvertService.DoPrintGlobalBroadcastOut(Context, msg);
        public void DoPrintRequest(string msg)=> Service.ConvertService.DoPrintRequest(Context, msg);

}