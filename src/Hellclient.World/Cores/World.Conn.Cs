using Hellclient.World.Types;

namespace Hellclient.World.Cores;

public partial class World
{
        public void DoConnectServer() => Service.ConnService.Connect(Context);
        public void DoCloseServer() => Service.ConnService.Close(Context);
        public bool GetConnConnected() => Service.ConnService.IsConnected(Context);
        public void DoPrint(string msg) => Service.ConvertService.DoPrint(Context, msg);
        public void DoPrintSystem(string msg) => Service.ConvertService.DoPrintSystem(Context, msg);
        public void DoPrintGlobalBroadcastIn(string msg)=> Service.ConvertService.DoPrintGlobalBroadcastIn(Context, msg);
        public void DoPrintLocalBroadcastOut(string msg)=> Service.ConvertService.DoPrintLocalBroadcastOut(Context, msg);
        public void DoPrintGlobalBroadcastOut(string msg)=> Service.ConvertService.DoPrintGlobalBroadcastOut(Context, msg);
        public void DoPrintRequest(string msg)=> Service.ConvertService.DoPrintRequest(Context, msg);

}