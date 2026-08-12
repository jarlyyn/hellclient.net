using Hellclient.Core.Bootstrappers;
using Hellclient.World.Configs;
using Hellclient.Core.Cores;
using Hellclient.Core.WebApp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Hellclient.WebUI;

public class WebUI
{
    public static WebUI Instance { get; set; } = new WebUI();
    public Prophet Prophet { get; set; } = AppCore.Instance.Prophet;
    public void Init()
    {
        WebApp.Instance.OnInit += (sender, app) =>
        {
            BuildApp(app);
        };
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
        context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"WebSocket\"";
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
        app.Use(BasicAuthMiddleware);
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
            AppCore.Instance.Prophet.Enter(conn);
            await conn.Run();
        }
        else
        {
            ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
        }

    }
}