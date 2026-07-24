using Hellclient.World.Features.Services;
using Hellclient.World.Types;

namespace Hellclient.World.Cores;

public partial class World
{
        public void DoSendToConn(byte[] cmd) => Service.ConnService.Send(Context, cmd);
        public async Task DoConnectServer() => await Service.ConnService.Connect(Context);
        public async Task DoCloseServer() => await Service.ConnService.Close(Context);
        public byte[] GetConnBuffer() => Service.ConnService.GetBuffer(Context);
        public bool GetConnConnected() => Service.ConnService.IsConnected(Context);
        public async Task DoSend(Command cmd) => await Service.ConnService.DoSend(Context, cmd);
        public void DoPrint(string msg) => Service.ConnService.DoPrint(Context, msg);
        public void DoPrintSystem(string msg) => Service.ConnService.DoPrintSystem(Context, msg);
        public void DoPrintLocalBroadcastIn(string msg)=> Service.ConnService.DoPrintLocalBroadcastIn(Context, msg);
        public void DoPrintGlobalBroadcastIn(string msg)=> Service.ConnService.DoPrintGlobalBroadcastIn(Context, msg);
        public void DoPrintLocalBroadcastOut(string msg)=> Service.ConnService.DoPrintLocalBroadcastOut(Context, msg);
        public void DoPrintGlobalBroadcastOut(string msg)=> Service.ConnService.DoPrintGlobalBroadcastOut(Context, msg);
        public void DoPrintSubneg(string msg)=> Service.ConnService.DoPrintSubneg(Context, msg);
        public void DoPrintRequest(string msg)=> Service.ConnService.DoPrintRequest(Context, msg);
        public void DoPrintResponse(string msg)=> Service.ConnService.DoPrintResponse(Context, msg);

}