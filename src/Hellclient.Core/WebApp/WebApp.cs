using Hellclient.Core.Configs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Hellclient.Core.WebApp;

public class WebApp
{
    public static WebApp Instance { get; set; } = new WebApp();
    private WebApplication? App { get; set; } = null;
    public EventHandler<WebApplication>? OnInit { get; set; }
    public void Start(string ListenAddress)
    {
        if (App == null)
        {
            var app = buildApp();
            App = app;
            OnInit?.Invoke(this, app);
            Task.Run(async () => await app.RunAsync(ListenAddress));
        }
    }
    public void Stop()
    {
        App?.StopAsync();
        App?.DisposeAsync();
        App = null;
    }
    private WebApplication buildApp()
    {
        var builder = WebApplication.CreateSlimBuilder();
        builder.Logging.ClearProviders();
        var app = builder.Build();
        app.UseStatusCodePages();
        var webSocketOptions = new WebSocketOptions
        {
            KeepAliveInterval = TimeSpan.FromMinutes(2)
        };
        app.UseWebSockets(webSocketOptions);
        return app;
    }


}