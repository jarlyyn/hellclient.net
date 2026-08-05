using Hellclient.Core.Features.Services;
using Hellclient.Core.Features.States;
using Hellclient.World.Cores;

namespace Hellclient.Core.Cores;

public class Titan
{
    public ITitanService Service { get; set; } = new TitanService();
    public required TitanContext Context { private get; init; }

    public IWorld? NewWorld(string id) => Service.NewWorld(Context, id);
    public IWorld? World(string id) => Service.World(Context, id);

    public async Task<bool> OpenWorld(string id) => await Service.OpenWorld(Context, id);

}