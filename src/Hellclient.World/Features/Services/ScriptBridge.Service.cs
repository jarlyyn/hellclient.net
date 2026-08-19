using Hellclient.World.Cores;
using Hellclient.World.Features.Repo;
using Hellclient.World.States;
namespace Hellclient.World.Features.Services;

public interface IScriptBridgeService
{
    public void InstallTo(WorldContext context);
    public void Save(WorldContext context);
    public void OpenScript(WorldContext context);
    public void Unload(WorldContext context);
    public void Load(WorldContext context);
    public void Reload(WorldContext context);
    public void UseScript(WorldContext context, string id);


}

public class ScriptBridgeService : IScriptBridgeService
{
    public ILogService LogService { get; set; } = new LogService();
    public IConfigService ConfigService { get; set; } = new ConfigService();
    public IScriptFileRepo ScriptFileRepo { get; set; } = new ScriptFileRepo();
    public IAutomationService AutomationService { get; set; } = new AutomationService();
    public IHUDService HudService { get; set; } = new HUDService();
    public IInfoService InfoService { get; set; } = new InfoService();
    public IScriptService ScriptService { get; set; } = new ScriptService();
    public void InstallTo(WorldContext context)
    {
        context.EventBus.ReadyEvent += (s, e) => _ready(context);
    }
    public void Save(WorldContext context)
    {
        _save(context);
    }
    private void _save(WorldContext context)
    {
        var id = ConfigService.GetScriptID(context);
        if (id == "")
        {
            return;
        }
        var timers = AutomationService.GetTimersByType(context, false);
        timers.Sort((a, b) => a.CompareTo(b));
        context.Script.Data.Timers = timers;
        var alias = AutomationService.GetAliasesByType(context, false);
        alias.Sort((a, b) => a.CompareTo(b));
        context.Script.Data.Aliases = alias;
        var triggers = AutomationService.GetTriggersByType(context, false);
        triggers.Sort((a, b) => a.CompareTo(b));
        context.Script.Data.Triggers = triggers;
        ScriptFileRepo.SaveScriptData(context, context.Script.Data, id);
    }
    public void OpenScript(WorldContext context)
    {
        _open(context);
    }
    private void _open(WorldContext context)
    {
        var id = ConfigService.GetScriptID(context);
        if (id == "")
        {
            return;
        }
        Types.ScriptData? data = null;
        try
        {
            data = ScriptFileRepo.LoadScriptData(context, id);
        }
        catch (Exception ex)
        {
            LogService.HandleScriptError(context, ex);
            return;
        }
        if (data == null)
        {
            return;
        }
        context.Script.Data = data;
        data.Timers.ForEach(t => t.SetByUser(false));
        data.Aliases.ForEach(t => t.SetByUser(false));
        data.Triggers.ForEach(t => t.SetByUser(false));
        AutomationService.AddTimers(context, data.Timers);
        AutomationService.AddAliases(context, data.Aliases);
        AutomationService.AddTriggers(context, data.Triggers);
    }
    public void Unload(WorldContext context)
    {
        _unload(context);
    }
    private void _unload(WorldContext context)
    {
        InfoService.SetSummary(context, []);
        HudService.SetSize(context, 0);
        ScriptService.SetCreator(context, "", "");
        AutomationService.DoDeleteTimerByType(context, false);
        AutomationService.DoDeleteAliasByType(context, false);
        AutomationService.DoDeleteTriggerByType(context, false);
        AutomationService.DeleteTemporaryTimers(context);
        AutomationService.DoDeleteTemporaryAliases(context);
        AutomationService.DoDeleteTemporaryTriggers(context);
        context.Script.Reset();
    }
    public void _load(WorldContext context)
    {
        HudService.SetSize(context, 0);
        _open(context);
        var data = ScriptService.ScriptData(context);
        context.Script.Engine = context.EngineCreator.Invoke(data.Type);
        context.Script.Engine.Open();
        return;
    }
    public void Load(WorldContext context)
    {
        _load(context);
    }
    public void Reload(WorldContext context)
    {
        _unload(context);
        _load(context);
    }
    private void _ready(WorldContext context)
    {
        Load(context);
        context.EventBus.StatusEvent?.Invoke(this, context.Script.Status);
    }
    private void _beforeClose(WorldContext context)
    {
        Unload(context);
    }
    public void UseScript(WorldContext context, string id)
    {
        _unload(context);
        ConfigService.SetScriptID(context, id);
        try
        {
            Load(context);
        }
        catch (Exception ex)
        {
            LogService.HandleScriptError(context, ex);
        }
    }
}