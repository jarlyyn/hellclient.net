using Hellclient.World.States;
using Hellclient.World.Types;
using Path = System.IO.Path;
using Timer = Hellclient.World.Types.Timer;

namespace Hellclient.World.Features.Services;

public interface IScriptService
{
    //脚本具体功能，底层接口，供其他服务调用。
    public void InstallTo(WorldContext context);
    public void Run(WorldContext context, string script);
    public void SendAlias(WorldContext context, string message, Alias alias, MatchResult matchResult);
    public void SendTrigger(WorldContext context, Line line, Trigger trigger, MatchResult matchResult);
    public void SetStatus(WorldContext context, string status);
    public void SetCreator(WorldContext context, string creator, string type);
    public void SendTimer(WorldContext context, Timer timer);
    public void SendHUDClick(WorldContext context, Click click);
    public void HandleFocus(WorldContext context);
    public void HandleLoseFocus(WorldContext context);
    public bool HandleBuffer(WorldContext context, byte[] buffer);
    public bool HandleLine(WorldContext context, string line);
    public void HandleAfterLine(WorldContext context, string line);
    public bool HandleSend(WorldContext context, string message);
    public void ReloadPermissions(WorldContext context);
    public PlainOptions PluginOptions(WorldContext context);
    public ScriptData ScriptData(WorldContext context);
    public CreatorInfo CreatorAndType(WorldContext context);
    public string GetStatus(WorldContext context);
    public List<RequiredParam> GetRequiredParams(WorldContext context);
    public string GetScriptType(WorldContext context);
    public void SendResponse(WorldContext context, Message msg);
    public void SendCallback(WorldContext context, Callback cb);
    public void Assist(WorldContext context);
    public void KeyUp(WorldContext context, string key);

}

public class ScriptService : IScriptService
{
    public IPathService PathService { get; set; } = new PathService();
    public IConfigService ConfigService { get; set; } = new ConfigService();
    public void InstallTo(WorldContext context)
    {

    }

    public void Run(WorldContext context, string script)
    {
        SetCreator(context, "run", "");
        context.Script.Engine.Run(script);
    }
    public void SendAlias(WorldContext context, string message, Alias alias, MatchResult matchResult)
    {
        SetCreator(context, "alias", alias.Script == "" ? $"#{alias.ID}" : alias.Script);
        context.Script.Engine.OnAlias(message, alias, matchResult);
    }
    public void SendTrigger(WorldContext context, Line line, Trigger trigger, MatchResult matchResult)
    {
        SetCreator(context, "trigger", trigger.Script == "" ? $"#{trigger.ID}" : trigger.Script);
        context.Script.Engine.OnTrigger(line, trigger, matchResult);
    }
    public void SendTimer(WorldContext context, Timer timer)
    {
        SetCreator(context, "timer", timer.Script == "" ? $"#{timer.ID}" : timer.Script);
        context.Script.Engine.OnTimer(timer);
    }
    public void SendHUDClick(WorldContext context, Click click)
    {
        SetCreator(context, "hudclick", "");
        context.Script.Engine.OnHUDClick(click);
    }
    public void SendResponse(WorldContext context, Message msg)
    {
        SetCreator(context, "response", "");
        context.Script.Engine.OnResponse(msg);
    }
    public void SendCallback(WorldContext context, Callback cb)
    {
        SetCreator(context, "callback", "");
        context.Script.Engine.OnCallback(cb);
    }
    public void Assist(WorldContext context)
    {
        var onassist = context.Script.Data.OnAssist;
        if (onassist == "")
        {
            return;
        }
        SetCreator(context, "assist", "");
        context.Script.Engine.OnAssist(onassist);
    }
    public void KeyUp(WorldContext context, string key)
    {
        SetCreator(context, "keyup", "");
        context.Script.Engine.OnKeyUp(key);
    }
    public void HandleFocus(WorldContext context)
    {
        SetCreator(context, "focus", "");
        context.Script.Engine.OnFocus();
    }
    public void HandleLoseFocus(WorldContext context)
    {
        SetCreator(context, "losefocus", "");
        context.Script.Engine.OnLoseFocus();
    }
    public bool HandleBuffer(WorldContext context, byte[] buffer)
    {
        SetCreator(context, "buffer", "");
        return context.Script.Engine.OnBuffer(buffer);
    }
    public void SetCreator(WorldContext context, string creator, string type)
    {
        context.Script.SetCreator(creator, type);
    }
    public void ReloadPermissions(WorldContext context)
    {
        _reloadPermissions(context);
    }
    private void _reloadPermissions(WorldContext context)
    {
        context.Script.Options.Location = Path.Combine(PathService.GetScriptPath(context), ConfigService.GetScriptID(context), "script");
        context.Script.Options.Trusted = ConfigService.GetTrusted(context);
        context.Script.Options.Permissions = ConfigService.GetPermissions(context);
    }
    public PlainOptions PluginOptions(WorldContext context)
    {
        var modpath = "";
        var home = PathService.GetScriptHome(context);
        modpath = Path.Combine(modpath, ConfigService.GetScriptID(context));//这里似乎有bug
        context.Script.Options.Home = home;
        context.Script.Options.ModPath = modpath;
        _reloadPermissions(context);
        return context.Script.Options;
    }
    public ScriptData ScriptData(WorldContext context)
    {
        return context.Script.Data;
    }
    public CreatorInfo CreatorAndType(WorldContext context)
    {
        return context.Script.CreatorAndType();
    }
    public string GetStatus(WorldContext context)
    {
        return context.Script.Status;
    }
    public void SetStatus(WorldContext context, string status)
    {
        context.Script.Status = status;
        context.EventBus.StatusEvent?.Invoke(this, status);
    }
    public List<RequiredParam> GetRequiredParams(WorldContext context)
    {
        return context.Script.Data.RequiredParams;
    }
    public string GetScriptType(WorldContext context)
    {
        return context.Script.CanRun() ? context.Script.Data.Type : "";
    }
    public bool HandleLine(WorldContext context, string line)
    {
        return context.Script.Engine.OnLine(line);
    }
    public void HandleAfterLine(WorldContext context, string line)
    {
        context.Script.Engine.OnAfterLine(line);
    }
    public bool HandleSend(WorldContext context, string message)
    {
        return context.Script.Engine.OnSend(message);
    }

}