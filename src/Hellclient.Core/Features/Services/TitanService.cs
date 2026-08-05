using System.ComponentModel;
using System.Text;
using Hellclient.Core.Features.States;
using Hellclient.Core.Helpers;
using Hellclient.Core.Infras.Components;
using Hellclient.Core.Types;
using Hellclient.World.Configs;
using Hellclient.World.Cores;
using Hellclient.World.Helpers;
using Hellclient.World.Types;
using Hellclient.World.Utils;
using Path = System.IO.Path;

namespace Hellclient.Core.Features.Services;

public interface ITitanService
{
    IWorld? World(TitanContext context, string id);
    IWorld? NewWorld(TitanContext context, string id);
    Task<bool> OpenWorld(TitanContext context, string id);
}
//World管理类，用来管理现有所有的游戏
public class TitanService : ITitanService
{
    public const string Ext = ".toml";
    public IWorldFactory WorldFactory { get; set; } = new WorldFactory();
    private IWorld? _find(TitanContext context, string id)
    {
        return context.Worlds.TryGetValue(id, out var world) ? world : null;
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
        var world = _find(context, id);
        if (world != null)
        {
            return world;
        }
        world = WorldFactory.CreateWorld(id, createPaths(context, id));
        context.Worlds[id] = world;
        return world;
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
    // public void  HandleCmdNotOpened() {
    // 	list, err := t.ListNotOpened()
    // 	if err != nil {
    // 		return
    // 	}
    // 	runtime.GC()
    // 	msg.PublishNotOpened(t, list)
    // }
    // public void  HandleCmdOpen(TitanContext context, string id) bool {
    // 	ok, err := t.OpenWorld(id)
    // 	if err != nil && !os.IsNotExist(err) {
    // 		util.LogError(err)
    // 		return false
    // 	}
    // 	w := t.World(id)
    // 	if w != nil {
    // 		w.UpdateLastActive()
    // 	}
    // 	return ok
    // }
    // public void  HandleCmdSave(TitanContext context, string id) {
    //         var w = World(context, id);
    //         if (w != null) {
    //             w.HandleCmdError(context.SaveWorld(id));
    //         }
    //     }
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
    // public void  HandleCmdListScriptInfo(TitanContext context) {
    //         var info = context.ListScripts();
    //         MsgHelper.PublishScriptInfoList(context.EventBus, info);
    //     }
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
    // private onSave(TitanContext context, IWorld world)
    // {
    //     	t.SaveWorld(b.ID)

    // }
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

}