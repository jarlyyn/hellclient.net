using Hellclient.Core.Configs;
using Hellclient.Core.WebApp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Hellclient.WebUI;

public class WebUI
{
    public static WebUI Instance { get; set; } = new WebUI();
    public void Init()
    {
        WebApp.Instance.OnInit += (sender, app) =>
        {
            BuildApp(app);
        };
    }
    public void BuildApp(WebApplication app)
    {
        app.Map("/ws", WSAction);
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
                Path.Combine(Deployment.Instance.ResourcesPath, "public")),
            RequestPath = "/public"
        });
        app.MapGet("/", async context =>
        {
            context.Response.ContentType = "text/html";
            await context.Response.SendFileAsync(Path.Combine(Deployment.Instance.ResourcesPath, "defaultui", "index.html"));
        });
    }
    public EventHandler<WebsocketConnection>? OnWS { get; set; }
    public async Task WSAction(HttpContext ctx)
    {
        if (ctx.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await ctx.WebSockets.AcceptWebSocketAsync();
            var conn = new WebsocketConnection(webSocket);
            OnWS?.Invoke(this, conn);
            await conn.Run();
        }
        else
        {
            ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
        }

    }
}