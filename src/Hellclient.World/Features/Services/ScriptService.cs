using Hellclient.World.States;
using Hellclient.World.Types;
using Path = System.IO.Path;
using Timer = Hellclient.World.Types.Timer;

namespace Hellclient.World.Features.Services;

public interface IScriptService
{
    public void InstallTo(WorldContext context);
    public void Run(WorldContext context, string script);
    public void SendAlias(WorldContext context, string message, Alias alias, MatchResult matchResult);
    public void SendTrigger(WorldContext context, Line line, Trigger trigger, MatchResult matchResult);
    public void SetStatus(WorldContext context, string status);
    public void SetCreator(WorldContext context, string creator, string type);
    public void SendTimer(WorldContext context, Timer timer);
    public void SendHUDClick(WorldContext context, Click click);
    public void SendBroadcast(WorldContext context, Broadcast bc);
    public void HandleFocus(WorldContext context);
    public void HandleLoseFocus(WorldContext context);
    public bool HandleBuffer(WorldContext context, byte[] buffer);
    public bool HandleSubneg(WorldContext context, byte[] data);
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
    public IConvertService ConvertService { get; set; } = new ConvertService();
    public void InstallTo(WorldContext context)
    {
        // Install script service to the world context
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
    private bool _verifyChannel(WorldContext context, string channel)
    {
        if (channel == "" || context.Script.Data.OnBroadcast == "")
        {
            return false;
        }
        return channel == context.Script.Data.Channel;
    }
    public void SendBroadcast(WorldContext context, Broadcast bc)
    {
        if (!_verifyChannel(context, bc.Channel))
        {
            return;
        }
        SetCreator(context, "broadcast", "");
        if (ConfigService.GetShowBroadcast(context))
        {
            if (bc.Global)
            {
                ConvertService.DoPrintGlobalBroadcastIn(context, bc.Message);
            }
            else
            {
                ConvertService.DoPrintLocalBroadcastIn(context, bc.Message);
            }
        }
        context.Script.Engine.OnBroadCast(bc);

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
    public bool HandleSubneg(WorldContext context, byte[] data)
    {
        if (data.Count() < 2)
        {
            return false;
        }
        if (ConfigService.GetShowSubneg(context))
        {
            ConvertService.DoPrintSubneg(context, $"[{data[0]}] {string.Join(" ", data.Skip(1).Select(b => b.ToString("X2")))}");
        }
        SetCreator(context, "subneg", "");
        return context.Script.Engine.OnBuffer(data);
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
}