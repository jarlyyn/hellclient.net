using Hellclient.Core.Configs;
using Hellclient.Core.Features.States;

namespace Hellclient.Core.Cores;

public class AppCore
{
    public static AppCore Instance { get; set; } = BuildDefault();
    public required Client Client { get; set; }
    public static AppCore BuildDefault()
    {
        var tctx = new TitanContext()
        {
            Deployment = Deployment.Instance,
        };
        var pctx = new ProphetContext()
        {
            Deployment = Deployment.Instance,
        };
        var ctx = new ClientContext()
        {
            Titan = tctx,
            Prophet = pctx
        };
        var titan = new Titan()
        {
            Context = ctx.Titan
        };
        var prophet = new Prophet()
        {
            Context = ctx.Prophet
        };
        var client = new Client()
        {
            Context = ctx,
            Titan = titan,
            Prophet = prophet
        };
        var app = new AppCore()
        {
            Client = client
        };
        return app;
    }
}