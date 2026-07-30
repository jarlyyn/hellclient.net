using Hellclient.World.States;
using Hellclient.World.Types;
using Hellclient.World.Cores;
using Timer = Hellclient.World.Types.Timer;


namespace Hellclient.World.Cores;

public partial class World
{
    public string DoEncode() => Service.LoaderService.Encode(Context);
    public void DoDecode(string data) => Service.LoaderService.Decode(Context, data);

}