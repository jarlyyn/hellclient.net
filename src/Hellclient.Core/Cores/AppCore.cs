using Hellclient.Core.Features.States;

namespace Hellclient.Core.Cores;

public class AppCore
{
    public static AppCore Instance { get; set; } = BuildDefault();
    public required Client Client { get; set; }
    public static AppCore BuildDefault()
    {
        var ctx= new ClientContext();
        var titan = new Titan()
        {
            Context = ctx.Titan
        };
        var client = new Client()
        {
            Context = ctx,
            Titan = titan
        };
        var app = new AppCore()
        {
            Client = client
        };
        return app;
    }
}