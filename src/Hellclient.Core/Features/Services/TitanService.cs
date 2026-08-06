using System.Text;
using System.Text.Json;
using Hellclient.Core.Features.States;
using Hellclient.Core.Helpers;
using Hellclient.Core.Infras.Components;
using Hellclient.Core.Types;
using Hellclient.World.Configs;
using Hellclient.World.Cores;
using Hellclient.World.Helpers;
using Hellclient.World.Infras.Components;
using Hellclient.World.Types;
using Hellclient.World.Utils;
using Tomlyn;
using Path = System.IO.Path;
using Timer = Hellclient.World.Types.Timer;

namespace Hellclient.Core.Features.Services;

public interface ITitanService
{
    IWorld? World(TitanContext context, string id);
    IWorld? NewWorld(TitanContext context, string id);
    Task<bool> OpenWorld(TitanContext context, string id);
    void SaveWorld(TitanContext context, string id);
    void AutoSaveWorld(TitanContext context, string id);
    public void HandleCmdConnect(TitanContext context, string id);
    public void HandleCmdDisconnect(TitanContext context, string id);
    public void HandleCmdSend(TitanContext context, string id, string msg);
    public void HandleCmdAllLines(TitanContext context, string id);
    public void HandleCmdNotOpened(TitanContext context);
    public Task<bool> HandleCmdOpen(TitanContext context, string id);
    public void ExecClients(TitanContext context);
    public void CloseWorld(TitanContext context, string id);
    public void HandleCmdSave(TitanContext context, string id);
    public void HandleCmdSaveScript(TitanContext context, string id);
    public void HandleCmdScriptInfo(TitanContext context, string id);
    public void HandleCmdListScriptInfo(TitanContext context);
    public void HandleCmdStatus(TitanContext context, string id);
    public void HandleCmdUseScript(TitanContext context, string id, string script);
    public void HandleCmdReloadScript(TitanContext context, string id);
    public void HandleCmdTimers(TitanContext context, string id, bool byuser);
    public bool? GetTimerType(TitanContext context, string world, string id);
    public void HandleCmdDeleteTimer(TitanContext context, string world, string id);
    public void HandleCmdLoadTimer(TitanContext context, string world, string id);
    public void HandleCmdAliases(TitanContext context, string id, bool byuser);
    public bool? GetAliasType(TitanContext context, string world, string id);
    public void HandleCmdDeleteAlias(TitanContext context, string world, string id);
    public void HandleCmdLoadAlias(TitanContext context, string world, string id);
    public void HandleCmdTriggers(TitanContext context, string id, bool byuser);
    public bool? GetTriggerType(TitanContext context, string world, string id);
    public void HandleCmdDeleteTrigger(TitanContext context, string world, string id);
    public void HandleCmdLoadTrigger(TitanContext context, string world, string id);
    public void HandleCmdParams(TitanContext context, string id);
    public void HandleCmdUpdateParam(TitanContext context, string id, string name, string value);
    public void HandleCmdUpdateParamComment(TitanContext context, string id, string name, string value);
    public void HandleCmdDeleteParam(TitanContext context, string id, string name);
    public void HandleCmdCallback(TitanContext context, string id, World.Types.Callback cb);
    public void HandleCmdAssist(TitanContext context, string id);
    public void HandleCmdAbout(TitanContext context);
    public void HandleCmdWorldSettings(TitanContext context, string id);
    public void HandleCmdScriptSettings(TitanContext context, string id);
    public void HandleCmdRequiredParams(TitanContext context, string id);
    public void HandleCmdDefaultServer(TitanContext context);
    public void HandleCmdDefaultCharset(TitanContext context);
    public void HandleCmdRequestPermissions(TitanContext context, Authorization a);
    public void HandleCmdRequestTrustDomains(TitanContext context, Authorization a);
    public void HandleCmdAuthorized(TitanContext context, string id);
    public void HandleCmdRevokeAuthorized(TitanContext context, string id);
    public void HandleCmdMasssend(TitanContext context, string id, string msg);
    public void HandleCmdFindHistory(TitanContext context, string id, int position);
    public void HandleCmdHUDClick(TitanContext context, string id, Click click);
    public void DoSortClients(TitanContext context, List<string> order);
    public void HandleCmdKeyUp(TitanContext context, string id, string key);
    public void HandleBatchCommand(TitanContext context, World.Types.BatchCommand bc);
    public void HandleCmdBatchCommandScripts(TitanContext context);
    public void ExecAPIversion(TitanContext context);
    public void ExecSwitchStatus(TitanContext context);
    public void Focus(TitanContext context, string id);
    public void HandleCmdLines(TitanContext context, string id);
    public void HandleCmdPrompt(TitanContext context, string id);
    public void HandleCmdHistory(TitanContext context, string id);
    public void HandleCmdHUDContent(TitanContext context, string id);
}
//World管理类，用来管理现有所有的游戏
public class TitanService : ITitanService
{
    public const string Ext = ".toml";
    public IWorldFactory WorldFactory { get; set; } = new WorldFactory();
    private IWorld? _find(TitanContext context, string id)
    {
        lock (context.Worlds)
        {
            var w = context.Worlds.TryGetValue(id, out var world) ? world : null;
            return w;
        }
    }
    public IWorld? World(TitanContext context, string id)
    {
        return _find(context, id);
    }
    private WorldPaths createPaths(TitanContext context, string id)
    {
        return new WorldPaths()
        {
            // TODO
        };
    }
    public IWorld? NewWorld(TitanContext context, string id)
    {
        lock (context.Worlds)
        {
            var world = _find(context, id);
            if (world != null)
            {
                return world;
            }
            world = WorldFactory.CreateWorld(id, createPaths(context, id));
            context.Worlds[id] = world;
            return world;
        }
    }
    public void Destory(TitanContext context, IWorld world)
    {
        world.Dispose();
        RemoveFrom(context, world);

    }
    public void Publish(TitanContext context, Types.Message message)
    {
        context.EventBus.MsgEvent?.Invoke(this, message);
    }
    private void onConnected(TitanContext context, IWorld world)
    {
        world.DoPrintSystem($"{DateTimeFormatter.Format(DateTime.Now)}  成功连接服务器");
        MsgHelper.PublishConnected(context.EventBus, world.ID);
    }
    private void onDisconnected(TitanContext context, IWorld world)
    {
        world.DoPrintSystem($"{DateTimeFormatter.Format(DateTime.Now)}  与服务器断开连接接");
        MsgHelper.PublishDisconnected(context.EventBus, world.ID);
    }
    private void onHUDUpdate(TitanContext context, IWorld world, DiffLines diff)
    {
        MsgHelper.PublishHUDUpdate(context.EventBus, world.ID, diff);
    }
    private void onHUDContent(TitanContext context, IWorld world, List<Line> content)
    {
        MsgHelper.PublishHUDContent(context.EventBus, world.ID, content);
    }
    private void onClientInfo(TitanContext context, IWorld world, ClientInfo info)
    {
        MsgHelper.PublishClientInfo(context.EventBus, world.ID, info);
    }

    private void onPrompt(TitanContext context, IWorld world, Line prompt)
    {
        MsgHelper.PublishPrompt(context.EventBus, world.ID, prompt);
    }

    private void onStatus(TitanContext context, IWorld world, string status)
    {
        MsgHelper.PublishStatus(context.EventBus, world.ID, status);
    }

    private void onHistory(TitanContext context, IWorld world, List<string> h)
    {
        MsgHelper.PublishHistory(context.EventBus, world.ID, h);
    }
    private void onScriptMessage(TitanContext context, IWorld world, object data)
    {
        MsgHelper.PublishScriptMessage(context.EventBus, world.ID, data);
    }

    private void onLines(TitanContext context, IWorld world, List<Line> lines)
    {
        MsgHelper.PublishLines(context.EventBus, world.ID, lines);
    }
    private void onLine(TitanContext context, IWorld world, Line line)
    {
        if (line.OmitFromOutput)
        {
            return;
        }
        MsgHelper.PublishLine(context.EventBus, world.ID, line);
    }
    private void onBroadcast(TitanContext context, IWorld world, Broadcast bc)
    {
        for (var i = 0; i < context.Worlds.Count; i++)
        {
            world.DoSendBroadcastToScript(bc);
        }

        if (bc.Global)
        {
            context.HellSwitch.Broadcast(Encoding.UTF8.GetBytes($"{bc.Channel} {bc.Message}"));
        }
    }
    private void onRequest(TitanContext context, IWorld world, World.Types.Message msg)
    {
        context.EventBus.RequestEvent?.Invoke(this, msg);
    }
    public void OnCreateSuccess(TitanContext context, string id)
    {
        MsgHelper.PublishCreateSuccess(context.EventBus, id);

        var world = context.Worlds.TryGetValue(id, out var w) ? w : null;
        world?.DoConnectServer();
    }
    public void OnUpdateSuccess(TitanContext context, string id)
    {
        MsgHelper.PublishUpdateSuccess(context.EventBus, id);
    }
    public void OnUpdateScriptSuccess(TitanContext context, string id)
    {
        MsgHelper.PublishUpdateScriptSuccess(context.EventBus, id);
    }

    public void OnCreateScriptFail(TitanContext context, List<FieldError> errors)
    {
        MsgHelper.PublishCreateScriptFail(context.EventBus, errors);
    }
    public void OnCreateScriptSuccess(TitanContext context, string id)
    {
        MsgHelper.PublishCreateScriptSuccess(context.EventBus, id);
    }

    public void HandleCmdConnect(TitanContext context, string id)
    {
        var w = World(context, id);
        w?.DoConnectServer();
    }
    public void HandleCmdDisconnect(TitanContext context, string id)
    {
        var w = World(context, id);
        w?.DoCloseServer();
    }
    public void HandleCmdStatus(TitanContext context, string id)
    {
        var w = World(context, id);
        if (w != null)
        {
            var status = w.GetStatus();
            MsgHelper.PublishStatus(context.EventBus, id, status);
        }
    }
    public void ExecSwitchStatus(TitanContext context)
    {
        MsgHelper.PublishSwitchStatusMessage(context.EventBus, context.HellSwitch.Status());
    }
    public void HandleCmdHistory(TitanContext context, string id)
    {
        var w = World(context, id);
        if (w != null)
        {
            var h = w.GetHistories();
            MsgHelper.PublishHistory(context.EventBus, id, h);
        }
    }
    public void Focus(TitanContext context, string id)
    {
        var w = World(context, id);
        if (w != null)
        {
            w.UpdateLastActive();
            w.HandleFocus();
        }
    }
    public void LoseFocus(TitanContext context, string id)
    {
        var w = World(context, id);
        w?.HandleLoseFocus();
    }
    public void HandleCmdHUDContent(TitanContext context, string id)
    {
        var w = World(context, id);
        if (w != null)
        {
            var lines = w.GetHUDContent();
            MsgHelper.PublishHUDContent(context.EventBus, id, lines);
        }
    }

    public void HandleCmdAllLines(TitanContext context, string id)
    {
        var w = World(context, id);
        if (w != null)
        {
            var alllines = w.GetCurrentLines();
            MsgHelper.PublishAllLines(context.EventBus, id, alllines);
        }
    }
    public void HandleCmdLines(TitanContext context, string id)
    {
        var w = World(context, id);
        if (w != null)
        {
            var alllines = w.GetCurrentLines();
            var start = alllines.Count - AppConfig.System.LinesPerScreen;
            if (start < 0)
            {
                start = 0;
            }
            MsgHelper.PublishLines(context.EventBus, id, alllines.GetRange(start, alllines.Count - start));
        }
    }
    public void HandleCmdPrompt(TitanContext context, string id)
    {
        var w = World(context, id);
        if (w != null)
        {
            var prompt = w.GetPrompt();
            MsgHelper.PublishPrompt(context.EventBus, id, prompt);
        }
    }
    public void HandleCmdNotOpened(TitanContext context)
    {
        var list = ListNotOpened(context);
        MsgHelper.PublishNotOpened(context.EventBus, list);
    }
    public async Task<bool> HandleCmdOpen(TitanContext context, string id)
    {
        var ok = await OpenWorld(context, id);
        var w = World(context, id);
        if (w != null)
        {
            w.UpdateLastActive();
        }
        return ok;
    }
    public void HandleCmdSave(TitanContext context, string id)
    {
        var w = World(context, id);
        if (w != null)
        {
            SaveWorld(context, id);
        }
    }
    public void HandleCmdSaveScript(TitanContext context, string id)
    {
        var w = World(context, id);
        w?.DoSaveScript();

    }
    public void HandleCmdScriptInfo(TitanContext context, string id)
    {
        var w = World(context, id);
        if (w != null)
        {
            var info = ScriptDataHelper.ConvertInfo(w.GetScriptID(), w.GetScriptData());
            MsgHelper.PublishScriptInfo(context.EventBus, id, info);
        }
    }
    public void HandleCmdListScriptInfo(TitanContext context)
    {
        var info = ListScripts(context);
        MsgHelper.PublishScriptInfoList(context.EventBus, info);
    }
    public void HandleCmdUseScript(TitanContext context, string id, string script)
    {
        var w = World(context, id);
        w?.DoUseScript(script);
    }
    public void HandleCmdReloadScript(TitanContext context, string id)
    {
        var w = World(context, id);
        w?.DoReloadScript();
    }
    public void HandleCmdCallback(TitanContext context, string id, World.Types.Callback cb)
    {
        var w = World(context, id);
        w?.DoSendCallbackToScript(cb);
    }
    public void HandleCmdAssist(TitanContext context, string id)
    {
        var w = World(context, id);
        w?.DoAssist();
    }
    public void HandleCmdAbout(TitanContext context)
    {
        MsgHelper.PublishVersionMessage(context.EventBus, AppVersion.Version.FullVersionCode());
    }
    public void HandleCmdDefaultServer(TitanContext context)
    {
        MsgHelper.PublishDefaultServerMessage(context.EventBus, AppConfig.System.DefaultServer);
    }

    public void HandleCmdDefaultCharset(TitanContext context)
    {
        MsgHelper.PublishDefaultCharsetMessage(context.EventBus, AppConfig.System.DefaultCharset);
    }
    public void ExecAPIversion(TitanContext context)
    {
        MsgHelper.PublishAPIVersionMessage(context.EventBus, AppVersion.APIVersion);
    }
    public void ExecClients(TitanContext context)
    {
        var result = new List<ClientInfo>();
        context.Worlds.ToList().ForEach(kv =>
        {
            result.Add(kv.Value.GetClientInfo()!);
        });
        result.Sort((a, b) => a.CompareTo(b));
        MsgHelper.PublishClients(context.EventBus, result);
    }
    private void onSave(TitanContext context, IWorld world)
    {
        SaveWorld(context, world.ID);
    }
    public void RequestPermissions(TitanContext context, IWorld world, Authorization a)
    {
        var w = World(context, world.ID);
        if (w != null)
        {
            MsgHelper.PublishRequestPermissions(context.EventBus, world.ID, a);
        }
    }
    public void RequestTrustDomains(TitanContext context, IWorld world, Authorization a)
    {
        var w = World(context, world.ID);
        if (w != null)
        {
            MsgHelper.PublishRequestTrustDomains(context.EventBus, world.ID, a);
        }
    }
    public void InstallTo(TitanContext context, IWorld world)
    {
        world.EventBus.ConnectedEvent += (sender, e) => onConnected(context, world);
        world.EventBus.DisconnectedEvent += (sender, e) => onDisconnected(context, world);
        world.EventBus.LineEvent += (sender, e) => onLine(context, world, e);
        world.EventBus.PromptEvent += (sender, e) => onPrompt(context, world, e);
        world.EventBus.StatusEvent += (sender, e) => onStatus(context, world, e);
        world.EventBus.HistoriesEvent += (sender, e) => onHistory(context, world, e);
        world.EventBus.LinesEvent += (sender, e) => onLines(context, world, e);
        world.EventBus.BroadcastEvent += (sender, e) => onBroadcast(context, world, e);
        world.EventBus.RequestEvent += (sender, e) => onRequest(context, world, e);
        world.EventBus.ScriptMessageEvent += (sender, e) => onScriptMessage(context, world, e);
        world.EventBus.HUDContentEvent += (sender, e) => onHUDContent(context, world, e);
        world.EventBus.HUDUpdateEvent += (sender, e) => onHUDUpdate(context, world, e);
        world.EventBus.ClientInfoEvent += (sender, e) => onClientInfo(context, world, e);
        world.EventBus.RequestPermissionsEvent += (sender, e) => RequestPermissions(context, world, e);
        world.EventBus.RequestTrustDomainsEvent += (sender, e) => RequestTrustDomains(context, world, e);
    }
    public void RemoveFrom(TitanContext context, IWorld world)
    {
        world.EventBus.ConnectedEvent = null;
        world.EventBus.DisconnectedEvent = null;
        world.EventBus.LineEvent = null;
        world.EventBus.PromptEvent = null;
        world.EventBus.StatusEvent = null;
        world.EventBus.HistoriesEvent = null;
        world.EventBus.LinesEvent = null;
        world.EventBus.BroadcastEvent = null;
        world.EventBus.RequestEvent = null;
        world.EventBus.ScriptMessageEvent = null;
        world.EventBus.HUDContentEvent = null;
        world.EventBus.HUDUpdateEvent = null;
        world.EventBus.ClientInfoEvent = null;
        world.EventBus.RequestPermissionsEvent = null;
        world.EventBus.RequestTrustDomainsEvent = null;
    }
    public string GetWorldPath(TitanContext context, string id)
    {
        return Path.Combine(context.Deployment.WorldsPath, $"{id}{Ext}");
    }
    public bool IsWorldExist(TitanContext context, string id)
    {
        var path = GetWorldPath(context, id);
        return File.Exists(path);
    }
    private void saveWorld(TitanContext context, string id, bool isautosave)
    {
        var w = World(context, id);
        if (w == null)
        {
            return;
        }
        if (isautosave && !w.GetAutoSave())
        {
            return;
        }
        var data = w.DoEncode();
        File.WriteAllText(GetWorldPath(context, id), data, Encoding.UTF8);
    }

    public void SaveWorld(TitanContext context, string id)
    {
        saveWorld(context, id, false);
    }
    public void AutoSaveWorld(TitanContext context, string id)
    {
        saveWorld(context, id, true);
    }
    public async Task<bool> OpenWorld(TitanContext context, string id)
    {
        if (context.Worlds.ContainsKey(id))
        {
            return false;
        }
        string data;
        try
        {
            data = File.ReadAllText(GetWorldPath(context, id), Encoding.UTF8);
        }
        catch (Exception e)
        {
            return false;
        }
        var world = WorldFactory.CreateWorld(id, createPaths(context, id));
        try
        {
            world?.DoDecode(data);
        }
        catch (Exception e)
        {
            return false;
        }
        InstallTo(context,world!);
        context.Worlds[id] = world!;
        world!.EventBus.ReadyEvent!.Invoke(this, EventArgs.Empty);
        _ = world!.DoConnectServer();
        return true;
    }
    private string prefixedName(string name, bool byuser)
    {
        return $"{(byuser ? Prefix.PrefixByUser : Prefix.PrefixByScript)}{name}";
    }

    public bool IsAliasNameAvaliable(TitanContext context, string id, string name, bool byuser)
    {
        var world = World(context, id);
        if (world == null)
        {
            return false;
        }
        name = prefixedName(name, byuser);
        return world.HasNamedAlias(name);
    }
    public bool DoCreateAlias(TitanContext context, string id, Alias alias)
    {
        var world = World(context, id);
        if (world != null)
        {
            return world.AddAlias(alias, false);
        }
        return false;
    }
    public int DoUpdateAlias(TitanContext context, string id, Alias alias)
    {
        var world = World(context, id);
        if (world != null)
        {
            return world.DoUpdateAlias(alias);
        }
        return MushString.UpdateFailNotFound;
    }
    public void OnCreateAliasSuccess(TitanContext context, string world, string id)
    {
        MsgHelper.PublishCreateAliasSuccess(context.EventBus, world, id);
    }
    public void OnUpdateAliasSuccess(TitanContext context, string world, string id)
    {
        MsgHelper.PublishUpdateAliasSuccess(context.EventBus, world, id);
    }
    public void HandleCmdAliases(TitanContext context, string id, bool byuser)
    {
        var world = World(context, id);
        if (world != null)
        {
            var aliases = world.GetAliasesByType(byuser);
            aliases.Sort();
            if (byuser)
            {
                MsgHelper.PublishUserAliases(context.EventBus, id, aliases);
            }
            else
            {
                MsgHelper.PublishScriptAliases(context.EventBus, id, aliases);
            }
        }
    }
    public void HandleCmdDeleteAlias(TitanContext context, string world, string id)
    {
        var w = World(context, world);
        if (w != null)
        {
            var itemtype = GetAliasType(context, world, id);
            w.DoDeleteAlias(id);
            if (itemtype != null && itemtype.Value)
            {
                AutoSaveWorld(context, id);
            }
        }
    }
    public bool? GetAliasType(TitanContext context, string world, string id)
    {
        var w = World(context, world);
        if (w != null)
        {
            var alias = w.GetAlias(id);
            if (alias != null)
            {
                var result = alias.ByUser();
                return result;
            }
        }
        return null;
    }

    public void HandleCmdLoadAlias(TitanContext context, string world, string id)
    {
        var w = World(context, world);
        if (w != null)
        {
            var alias = w.GetAlias(id);
            if (alias != null)
            {
                MsgHelper.PublishAlias(context.EventBus, world, alias);
            }
        }
    }
    public bool IsTriggerNameAvaliable(TitanContext context, string id, string name, bool byuser)
    {
        var world = World(context, id);
        if (world != null)
        {
            name = prefixedName(name, byuser);
            return world.HasNamedTrigger(name);
        }
        return false;
    }
    public bool DoCreateTrigger(TitanContext context, string id, Trigger trigger)
    {
        var w = World(context, id);
        if (w != null)
        {
            return w.AddTrigger(trigger, false);
        }
        return false;
    }
    public int DoUpdateTrigger(TitanContext context, string id, Trigger trigger)
    {
        var w = World(context, id);
        if (w != null)
        {
            return w.DoUpdateTrigger(trigger);
        }
        return MushString.UpdateFailNotFound;
    }
    public void OnCreateTriggerSuccess(TitanContext context, string world, string id)
    {
        MsgHelper.PublishCreateTriggerSuccess(context.EventBus, world, id);
    }
    public void OnUpdateTriggerSuccess(TitanContext context, string world, string id)
    {
        MsgHelper.PublishUpdateTriggerSuccess(context.EventBus, world, id);
    }
    public void HandleCmdTriggers(TitanContext context, string id, bool byuser)
    {
        var world = World(context, id);
        if (world != null)
        {
            var triggers = world.GetTriggersByType(byuser);
            triggers.Sort();
            if (byuser)
            {
                MsgHelper.PublishUserTriggers(context.EventBus, id, triggers);
            }
            else
            {
                MsgHelper.PublishScriptTriggers(context.EventBus, id, triggers);
            }
        }
    }
    public void HandleCmdDeleteTrigger(TitanContext context, string world, string id)
    {
        var w = World(context, world);
        if (w != null)
        {
            var itemtype = GetTriggerType(context, world, id);
            w.DoDeleteTrigger(id);
            if (itemtype != null && itemtype.Value)
            {
                AutoSaveWorld(context, id);
            }
        }
    }
    public bool? GetTriggerType(TitanContext context, string world, string id)
    {
        var w = World(context, world);
        if (w != null)
        {
            var trigger = w.GetTrigger(id);
            if (trigger != null)
            {
                var result = trigger.ByUser();
                return result;
            }
        }
        return null;
    }
    public void HandleCmdLoadTrigger(TitanContext context, string world, string id)
    {
        var w = World(context, world);
        if (w != null)
        {
            var trigger = w.GetTrigger(id);
            if (trigger != null)
            {
                MsgHelper.PublishTrigger(context.EventBus, world, trigger);
            }
        }
    }
    public bool IsTimerNameAvaliable(TitanContext context, string id, string name, bool byuser)
    {
        var w = World(context, id);
        if (w != null)
        {
            name = prefixedName(name, byuser);
            return w.HasNamedTimer(name);
        }
        return false;
    }

    public bool DoCreateTimer(TitanContext context, string id, Timer timer)
    {
        var w = World(context, id);
        if (w != null)
        {
            return w.AddTimer(timer, false);
        }
        return false;
    }
    public int DoUpdateTimer(TitanContext context, string id, Timer timer)
    {
        var w = World(context, id);
        if (w != null)
        {
            return w.DoUpdateTimer(timer);
        }
        return MushString.UpdateFailNotFound;
    }
    public void OnCreateTimerSuccess(TitanContext context, string world, string id)
    {
        MsgHelper.PublishCreateTimerSuccess(context.EventBus, world, id);
    }
    public void OnUpdateTimerSuccess(TitanContext context, string world, string id)
    {
        MsgHelper.PublishUpdateTimerSuccess(context.EventBus, world, id);
    }

    public void HandleCmdTimers(TitanContext context, string id, bool byuser)
    {
        var w = World(context, id);
        if (w != null)
        {
            var timers = w.GetTimersByType(byuser);
            timers.Sort();
            if (byuser)
            {
                MsgHelper.PublishUserTimers(context.EventBus, id, timers);
            }
            else
            {
                MsgHelper.PublishScriptTimers(context.EventBus, id, timers);
            }
        }
    }
    public void HandleCmdDeleteTimer(TitanContext context, string world, string id)
    {
        var w = World(context, world);
        if (w != null)
        {
            var itemtype = GetTimerType(context, world, id);
            w.DoDeleteTimer(id);
            if (itemtype != null && itemtype.Value)
            {
                AutoSaveWorld(context, id);
            }
        }
    }
    public bool? GetTimerType(TitanContext context, string world, string id)
    {
        var w = World(context, world);
        if (w != null)
        {
            var timer = w.GetTimer(id);
            if (timer != null)
            {
                var result = timer.ByUser();
                return result;
            }
        }
        return null;
    }
    public void HandleCmdLoadTimer(TitanContext context, string world, string id)
    {
        var w = World(context, world);
        if (w != null)
        {
            var timer = w.GetTimer(id);
            if (timer != null)
            {
                MsgHelper.PublishTimer(context.EventBus, world, timer);
            }
        }
    }
    public void HandleCmdSend(TitanContext context, string id, string msg)
    {
        var w = World(context, id);
        if (w != null && msg != "")
        {
            w.AddHistory(msg);
            w.DoExecute(msg);
        }
    }
    public void HandleCmdMasssend(TitanContext context, string id, string msg)
    {
        var w = World(context, id);
        if (w != null)
        {
            var m = Command.Create(msg);
            m.History = false;
            w.DoMetronomeSend(m);
        }
    }
    public void HandleCmdFindHistory(TitanContext context, string id, int position)
    {
        if (position < 0)
        {
            return;
        }
        var w = World(context, id);
        if (w != null)
        {
            var h = w.GetHistories();
            if (position >= h.Count)
            {
                return;
            }
            MsgHelper.PublishFoundHistory(context.EventBus, id, new FoundHistory() { Position = position, Command = h[h.Count - 1 - position] });
        }
    }

    public void HandleCmdHUDClick(TitanContext context, string id, Click click)
    {
        var w = World(context, id);
        if (w != null)
        {
            w.DoSendHUDClickToScript(click);
        }
    }
    public void HandleCmdKeyUp(TitanContext context, string id, string key)
    {
        var w = World(context, id);
        if (w != null && key != "")
        {
            w.DoSendKeyUpToScript(key);
        }
    }
    public void DoSortClients(TitanContext context, List<string> order)
    {
        lock (context.Worlds)
        {
            var ordermap = new Dictionary<string, int>();
            int max = order.Count;
            int maxword = context.Worlds.Count;
            if (maxword > max)
            {
                max = maxword;
            }
            for (int k = 0; k < order.Count; k++)
            {
                var v = order[k];
                ordermap[v] = k - max;
            }
            foreach (var kvp in context.Worlds)
            {
                kvp.Value.SetPosition(ordermap[kvp.Key]);
            }
        }
    }
    public bool IsScriptExist(TitanContext context, string id)
    {
        var scriptPath = Path.Combine(context.ScriptPath, id);
        return File.Exists(scriptPath);
    }
    public List<WorldFile> ListNotOpened(TitanContext context)
    {
        lock (context.Worlds)
        {
            var result = new List<WorldFile>();
            var files = Directory.GetFiles(Path.Combine(context.WorldsPath));
            foreach (var file in files)
            {
                var name = Path.GetFileName(file);
                if (!name.EndsWith(Ext))
                {
                    continue;
                }
                var id = name.Substring(0, name.Length - Ext.Length);
                if (context.Worlds.ContainsKey(id))
                {
                    continue;
                }
                var info = new FileInfo(file);
                var data = File.ReadAllText(file);
                // Assuming toml.Unmarshal equivalent in C# is available
                var configdata = TomlSerializer.Deserialize<WorldData>(data, TomlContext.Default.WorldData)!;
                var ut = DateTimeFormatter.Format(info.LastWriteTime);
                result.Add(new WorldFile
                {
                    ID = id,
                    Name = configdata.Name,
                    LastUpdated = ut
                });
                configdata = null;
            }
            return result;
        }
    }
    public List<string> ListWorlds(TitanContext context)
    {
        var result = new List<string>();
        var files = Directory.GetFiles(Path.Combine(context.ScriptPath));
        foreach (var file in files)
        {
            var name = Path.GetFileName(file);
            if (name.EndsWith(Ext))
            {
                result.Add(name.Substring(0, name.Length - Ext.Length));
            }
        }
        return result;
    }
    public List<ScriptInfo> ListScripts(TitanContext context)
    {
        var result = new List<ScriptInfo>();
        var dirs = Directory.GetDirectories(Path.Combine(context.ScriptPath));
        foreach (var dir in dirs)
        {
            var id = Path.GetFileName(dir);
            if (IDRegexp.MatchString(id))
            {
                var file = Path.Combine(dir, "script.toml");
                if (!File.Exists(file))
                {
                    continue;
                }
                var data = File.ReadAllText(file);
                var d = TomlSerializer.Deserialize<ScriptData>(data, TomlContext.Default.ScriptData)!;
                result.Add(ScriptDataHelper.ConvertInfo(id, d));
            }
        }
        return result;
    }
    public void CloseWorld(TitanContext context, string id)
    {
        lock (context.Worlds)
        {

            var w = context.Worlds[id];
            if (w == null)
            {
                return;
            }
            context.Worlds.Remove(id);
            Destory(context, w);
        }
    }
    public void HandleCmdParams(TitanContext context, string id)
    {
        var w = World(context, id);
        if (w is null)
        {
            return;
        }
        var i = new ParamsInfo();
        i.Params = w.GetParams();
        i.ParamComments = w.GetParamComments();
        i.RequiredParams = w.GetRequiredParams();
        MsgHelper.PublishParamsinfo(context.EventBus, id, i);
    }
    public void HandleCmdDeleteParam(TitanContext context, string id, string name)
    {
        var w = World(context, id);
        if (w is null)
        {
            return;
        }
        w.DeleteParam(name);
        AutoSaveWorld(context, id);
        MsgHelper.PublishParamDeleted(context.EventBus, id, name);
        HandleCmdParams(context, id);
    }
    public void HandleCmdUpdateParam(TitanContext context, string id, string name, string value)
    {
        var w = World(context, id);
        if (w is null)
        {
            return;
        }
        w.SetParam(name, value);
        MsgHelper.PublishParamUpdated(context.EventBus, id, name);
        AutoSaveWorld(context, id);
        HandleCmdParams(context, id);
    }
    public void HandleCmdUpdateParamComment(TitanContext context, string id, string name, string value)
    {
        var w = World(context, id);
        if (w is null)
        {
            return;
        }
        w.SetParamComment(name, value);
        MsgHelper.PublishParamUpdated(context.EventBus, id, name);
        AutoSaveWorld(context, id);
        HandleCmdParams(context, id);
    }
    public void HandleCmdWorldSettings(TitanContext context, string id)
    {
        var w = World(context, id);
        if (w is null)
        {
            return;
        }
        var s = WorldDataHelper.ConvertSettings(id, w.GetWorldData());
        MsgHelper.PublishWorldSettingsMessage(context.EventBus, id, s);
    }
    public void HandleCmdScriptSettings(TitanContext context, string id)
    {
        var w = World(context, id);
        if (w is null)
        {
            return;
        }
        var s = ScriptDataHelper.ConvertSettings(w.GetScriptID(), w.GetScriptData());

        MsgHelper.PublishScriptSettingsMessage(context.EventBus, id, s);
    }
    public void HandleCmdRequiredParams(TitanContext context, string id)
    {
        var w = World(context, id);
        if (w is null)
        {
            return;
        }
        var p = new List<RequiredParam>();
        var s = w.GetScriptData();
        if (s != null)
        {
            p = s.RequiredParams;
        }
        MsgHelper.PublishRequiredParamsMessage(context.EventBus, id, p);
    }

    public void HandleCmdRequestPermissions(TitanContext context, Authorization a)
    {
        var w = World(context, a.World);
        if (w is null)
        {
            return;
        }
        var items = w.GetPermissions();

        foreach (var authed in a.Items)
        {
            foreach (var owned in items)
            {
                if (owned == authed)
                {
                    goto Next;
                }
            }
            items.Add(authed);
        Next:;
        }
        w.SetPermissions(items);
        w.DoReloadPermissions();
        if (a.Script != "")
        {
            w.DoRunScript(a.Script);
        }
    }

    public void HandleCmdRequestTrustDomains(TitanContext context, Authorization a)
    {
        var w = World(context, a.World);
        if (w is null)
        {
            return;
        }
        var trusted = w.GetTrusted();

        foreach (var authed in a.Items)
        {
            foreach (var owned in trusted.Domains)
            {
                if (owned == authed)
                {
                    goto Next;
                }
            }
            trusted.Domains.Add(authed);
        Next:;
        }
        w.SetTrusted(trusted);

        w.DoReloadPermissions();

        if (a.Script != "")
        {
            w.DoRunScript(a.Script);
        }
    }
    public void HandleCmdAuthorized(TitanContext context, string id)
    {
        var w = World(context, id);


        if (w is null)
        {
            return;
        }
        var a = new Authorized();
        var p = w.GetPermissions();
        var trusted = w.GetTrusted();
        a.Permissions.AddRange(p);
        a.Domains.AddRange(trusted.Domains);
        w.DoReloadPermissions();
        MsgHelper.PublishAuthorized(context.EventBus, id, a);
    }
    public void HandleCmdRevokeAuthorized(TitanContext context, string id)
    {
        var w = World(context, id);

        if (w is null)
        {
            return;
        }
        w.SetPermissions(new List<string>());
        var trusted = w.GetTrusted();
        trusted.Domains = new List<string>();
        w.SetTrusted(trusted);
        w.DoReloadPermissions();
        MsgHelper.PublishAuthorized(context.EventBus, id, new Authorized());
    }

    public void HandleCmdUpdateRequiredParams(TitanContext context, string id, List<RequiredParam> p)
    {
        var w = World(context, id);
        if (w is null)
        {
            return;

        }
        var data = w.GetScriptData();


        if (data != null)
        {
            data.RequiredParams = p;
            MsgHelper.PublishRequiredParamsMessage(context.EventBus, id, data.RequiredParams);

        }
    }

    public void HandleCmdBatchCommandScripts(TitanContext context)
    {

        var result = new List<string>();
        var resultmap = new Dictionary<string, bool> { };
        lock (context.Worlds)
        {
            foreach (var w in context.Worlds.ToList().Select(kv => kv.Value))
            {
                var sid = w.GetScriptID();


                if (!resultmap.ContainsKey(sid))
                {
                    resultmap[sid] = true;
                    result.Add(sid);
                }
            }
        }
        var bcs = new BatchCommandScripts();
        bcs.Scripts = result;
        MsgHelper.PublishBatchCommandScripts(context.EventBus, bcs);
    }
    public void NewScript(TitanContext context, string id, string scripttype)
    {
        // 	t.Locker.Lock()
        // 	defer t.Locker.Unlock()
        // 	ok, err := t.IsScriptExist(id)
        // 	if err != nil {
        // 		return err
        // 	}
        // 	if ok {
        // 		return errors.New("script exists")
        // 	}
        // 	err = os.MkdirAll(filepath.Join(t.Scriptpath, id, "script"), util.DefaultFolderMode)
        // 	if err != nil {
        // 		return err
        // 	}
        // 	data, err := os.ReadFile(world.ScriptTomlTemplates[scripttype])
        // 	if err != nil {
        // 		return err
        // 	}
        // 	err = os.WriteFile(filepath.Join(t.Scriptpath, id, "script.toml"), data, util.DefaultFileMode)
        // 	if err != nil {
        // 		return err
        // 	}
        // 	data, err = os.ReadFile(world.ScriptTemplates[scripttype])
        // 	if err != nil {
        // 		return err
        // 	}
        // 	err = os.WriteFile(filepath.Join(t.Scriptpath, id, "script", world.ScriptTargets[scripttype]), data, util.DefaultFileMode)
        // 	if err != nil {
        // 		return err
        // 	}
        // 	return nil
    }
    public void OnResponse(TitanContext context, World.Types.Message msg)
    {
        var w = World(context, msg.World);
        if (w is not null)
        {
            w.DoSendResponseToScript(msg);
        }
    }
    public void OnBatchCommandMessage(TitanContext context, World.Types.Message msg)
    {
        var bc = JsonSerializer.Deserialize(msg.Data, Infras.Components.JsonContext.Default.BatchCommand);
        HandleBatchCommand(context, bc);
    }

    public void HandleBatchCommand(TitanContext context, World.Types.BatchCommand bc)
    {
        lock (context.Worlds)
        {
            foreach (var w in context.Worlds.Values)
            {
                if (!w.GetIgnoreBatchCommand())
                {
                    var scriptid = w.GetScriptID();
                    foreach (var bcsid in bc.Scripts)
                    {
                        if (bcsid == "" || bcsid == scriptid)
                        {
                            w.DoExecute(bc.Command);
                            break;
                        }
                    }
                }
            }
        }
    }

    public void OnGlobalMessage(TitanContext context, byte[] msg)
    {
        var data = Encoding.UTF8.GetString(msg).Split(" ", 3);
        switch (data[0])
        {
            case "broadcast":
                if (data.Length == 3)
                {
                    var bc = Broadcast.CreateBroadcast(data[1], data[2], true);
                    lock (context.Worlds)
                    {
                        foreach (var v in context.Worlds.Values)
                        {
                            v.DoSendBroadcastToScript(bc);
                        }
                    }
                }
                break;
        }
    }

    public void OnSwitchStatusChange(TitanContext context, int status)
    {
        MsgHelper.PublishSwitchStatusMessage(context.EventBus, status);
    }
    public void Start(TitanContext context)
    {
        context.HellSwitch.OnGlobalMessage += (sender, e) => OnGlobalMessage(context, e);
        context.HellSwitch.OnSwitchStatusChange += (sender, e) => OnSwitchStatusChange(context, e);
        context.HellSwitch.Start();
    }
    public void Stop(TitanContext context)
    {
        context.HellSwitch.OnGlobalMessage = null;
        context.HellSwitch.OnSwitchStatusChange = null;
        context.HellSwitch.Stop();
        context.HellSwitch.Close();
    }
}