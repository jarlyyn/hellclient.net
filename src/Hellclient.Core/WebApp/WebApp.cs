using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using NWebDav.Server;
using Microsoft.Extensions.DependencyInjection;
using Hellclient.Core.Cores;
using Microsoft.AspNetCore.Http;
using Hellclient.Core.Bootstrappers;
using Hellclient.Core.Infras.Adapters;
using NWebDav.Server.Stores;
using Hellclient.World.Configs;

namespace Hellclient.Core.WebApp;



public class WebApp
{
    public Prophet Prophet { get; set; } = AppCore.Instance.Prophet;

    public static WebApp Instance { get; set; } = new WebApp();
    private WebApplication? App { get; set; } = null;
    public EventHandler<WebApplication>? OnInit { get; set; }
    public async Task Start(string ListenAddress)
    {
        if (App == null)
        {
            var app = BuildApp();
            App = app;
            OnInit?.Invoke(this, app);
            await app.RunAsync(ListenAddress);
        }
    }
    public void Stop()
    {
        App?.StopAsync();
        App?.DisposeAsync();
        App = null;
    }
    public async Task BasicAuthMiddleware(HttpContext context, Func<Task> next)
    {
        var authHeader = context.Request.Headers["Authorization"].ToString();
        var username = "";
        var password = "";
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Basic "))
        {
            var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
            var decodedCredentials = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            var parts = decodedCredentials.Split(':', 2);
            if (parts.Length == 2)
            {
                username = parts[0];
                password = parts[1];
            }
        }
        if (Prophet.CheckAuth(username, password))
        {
            await next.Invoke();
            return;
        }
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Hellclient\", charset=\"UTF-8\"";
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync("Unauthorized");
    }

    private WebApplication BuildApp()
    {
        var builder = WebApplication.CreateSlimBuilder();
        builder.Services.AddNWebDav(opts => opts.Filter = (ctx) =>
        {
            if (ctx.Request.Path.StartsWithSegments("/game"))
            {
                // ctx.Request.Path = ctx.Request.Path!.Value!.Substring("/game".Length);
                return true;
            }
            return false;
        });
        builder.Services.Configure<DiskStoreOptions>(options =>
        {
            options.BaseDirectory = Deployment.Instance.AppdataPath;
        });

        builder.Services.AddDiskStore<DiskStore>();
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.SetMinimumLevel(LogLevel.Error);
        var app = builder.Build();
        app.UseStatusCodePages();
        var webSocketOptions = new WebSocketOptions
        {
            KeepAliveInterval = TimeSpan.FromMinutes(2)
        };
        app.Use(BasicAuthMiddleware);
        app.UseWebSockets(webSocketOptions);
        app.UseNWebDav();
        return app;
    }


}