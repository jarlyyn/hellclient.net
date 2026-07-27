using Hellclient.World.States;
using Hellclient.World.Types;

namespace Hellclient.World.Features.Services;

public interface IConfigService
{
    public void InstallTo(WorldContext context);
    public int GetPosition(WorldContext context);
    public void SetPosition(WorldContext context, int position);
    public WorldData GetWorldData(WorldContext context);
    public long GetReadyAt(WorldContext context);
    public string GetPort(WorldContext context);
    public void SetPort(WorldContext context, string port);
    public string GetProxy(WorldContext context);
    public void SetProxy(WorldContext context, string proxy);
    public int GetQueueDelay(WorldContext context);
    public void SetQueueDelay(WorldContext context, int delay);
    public List<string> GetPermissions(WorldContext context);
    public void SetPermissions(WorldContext context, List<string> permissions);
    public Trusted GetTrusted(WorldContext context);
    public void SetTrusted(WorldContext context, Trusted trusted);
    public string GetHost(WorldContext context);
    public void SetHost(WorldContext context, string host);
    public string GetCharset(WorldContext context);
    public void SetCharset(WorldContext context, string charset);
    public string GetParam(WorldContext context, string key);
    public void SetParam(WorldContext context, string key, string value);
    public void DeleteParam(WorldContext context, string key);
    public Dictionary<string, string> GetParams(WorldContext context);
    public string GetParamComment(WorldContext context, string key);
    public void SetParamComment(WorldContext context, string key, string comment);
    public string GetScriptID(WorldContext context);
    public void SetScriptID(WorldContext context, string scriptID);
    public string GetName(WorldContext context);
    public void SetName(WorldContext context, string name);
    public string GetCommandStackCharacter(WorldContext context);
    public void SetCommandStackCharacter(WorldContext context, string character);
    public string GetScriptPrefix(WorldContext context);
    public void SetScriptPrefix(WorldContext context, string prefix);
    public bool GetShowBroadcast(WorldContext context);
    public void SetShowBroadcast(WorldContext context, bool showBroadcast);
    public bool GetShowSubneg(WorldContext context);
    public void SetShowSubneg(WorldContext context, bool showSubneg);
    public bool GetModEnabled(WorldContext context);
    public void SetModEnabled(WorldContext context, bool modEnabled);
    public bool GetAutoSave(WorldContext context);
    public void SetAutoSave(WorldContext context, bool autoSave);
    public bool GetIgnoreBatchCommand(WorldContext context);
    public void SetIgnoreBatchCommand(WorldContext context, bool ignoreBatchCommand);
    public Dictionary<string, string> GetParamComments(WorldContext context);
}

public class ConfigService : IConfigService
{
    public void InstallTo(WorldContext context)
    {
        context.EventBus.ReadyEvent += (sender, args) => OnReady(context);
    }
    public int GetPosition(WorldContext context)
    {
        return context.Config.Position;
    }
    public void SetPosition(WorldContext context, int position)
    {
        context.Config.Position = position;
    }
    public WorldData GetWorldData(WorldContext context)
    {
        return context.Config.Data;
    }
    public long GetReadyAt(WorldContext context)
    {
        return context.Config.ReadyAt;
    }
    public string GetPort(WorldContext context)
    {
        return context.Config.Data.Port;
    }
    public void SetPort(WorldContext context, string port)
    {
        context.Config.Data.Port = port;
    }
    public string GetProxy(WorldContext context)
    {
        return context.Config.Data.Proxy;
    }
    public void SetProxy(WorldContext context, string proxy)
    {
        context.Config.Data.Proxy = proxy;
    }
    public int GetQueueDelay(WorldContext context)
    {
        return context.Config.Data.QueueDelay;
    }
    public void SetQueueDelay(WorldContext context, int delay)
    {
        context.Config.Data.QueueDelay = delay;
        context.EventBus.QueueDelayUpdatedEvent?.Invoke(this, EventArgs.Empty);
    }
    public List<string> GetPermissions(WorldContext context)
    {
        return context.Config.Data.Permissions;
    }
    public void SetPermissions(WorldContext context, List<string> permissions)
    {
        context.Config.Data.Permissions = permissions;
    }
    public Trusted GetTrusted(WorldContext context)
    {
        return context.Config.Data.Trusted;
    }
    public void SetTrusted(WorldContext context, Trusted trusted)
    {
        context.Config.Data.Trusted = trusted;
    }
    public string GetHost(WorldContext context)
    {
        return context.Config.Data.Host;
    }
    public void SetHost(WorldContext context, string host)
    {
        context.Config.Data.Host = host;
    }
    public string GetCharset(WorldContext context)
    {
        return context.Config.Data.Charset;
    }
    public void SetCharset(WorldContext context, string charset)
    {
        context.Config.Data.Charset = charset;
    }
    public string GetParam(WorldContext context, string key)
    {
        if (context.Config.Data.Params.ContainsKey(key))
        {
            return context.Config.Data.Params[key];
        }
        return "";
    }
    public void SetParam(WorldContext context, string key, string value)
    {
        context.Config.Data.Params[key] = value;
    }
    public void DeleteParam(WorldContext context, string key)
    {
        if (context.Config.Data.Params.ContainsKey(key))
        {
            context.Config.Data.Params.Remove(key);
        }
    }
    public Dictionary<string, string> GetParams(WorldContext context)
    {
        return context.Config.Data.Params;
    }
    public string GetParamComment(WorldContext context, string key)
    {
        if (context.Config.Data.ParamComments.ContainsKey(key))
        {
            return context.Config.Data.ParamComments[key];
        }
        return "";
    }
    public void SetParamComment(WorldContext context, string key, string comment)
    {
        context.Config.Data.ParamComments[key] = comment;
    }
    public string GetScriptID(WorldContext context)
    {
        return context.Config.Data.ScriptID;
    }
    public void SetScriptID(WorldContext context, string scriptID)
    {
        context.Config.Data.ScriptID = scriptID;
    }
    public string GetName(WorldContext context)
    {
        return context.Config.Data.Name;
    }
    public void SetName(WorldContext context, string name)
    {
        context.Config.Data.Name = name;
    }
    public string GetCommandStackCharacter(WorldContext context)
    {
        return context.Config.Data.CommandStackCharacter;
    }
    public void SetCommandStackCharacter(WorldContext context, string character)
    {
        context.Config.Data.CommandStackCharacter = character;
    }
    public string GetScriptPrefix(WorldContext context)
    {
        return context.Config.Data.ScriptPrefix;
    }
    public void SetScriptPrefix(WorldContext context, string prefix)
    {
        context.Config.Data.ScriptPrefix = prefix;
    }
    public bool GetShowBroadcast(WorldContext context)
    {
        return context.Config.Data.ShowBroadcast;
    }
    public void SetShowBroadcast(WorldContext context, bool showBroadcast)
    {
        context.Config.Data.ShowBroadcast = showBroadcast;
    }
    public bool GetShowSubneg(WorldContext context)
    {
        return context.Config.Data.ShowSubneg;
    }
    public void SetShowSubneg(WorldContext context, bool showSubneg)
    {
        context.Config.Data.ShowSubneg = showSubneg;
    }
    public bool GetModEnabled(WorldContext context)
    {
        return context.Config.Data.ModEnabled;
    }
    public void SetModEnabled(WorldContext context, bool modEnabled)
    {
        context.Config.Data.ModEnabled = modEnabled;
    }
    public bool GetAutoSave(WorldContext context)
    {
        return context.Config.Data.AutoSave;
    }
    public void SetAutoSave(WorldContext context, bool autoSave)
    {
        context.Config.Data.AutoSave = autoSave;
    }
    public bool GetIgnoreBatchCommand(WorldContext context)
    {
        return context.Config.Data.IgnoreBatchCommand;
    }
    public void SetIgnoreBatchCommand(WorldContext context, bool ignoreBatchCommand)
    {
        context.Config.Data.IgnoreBatchCommand = ignoreBatchCommand;
    }
    private void OnReady(WorldContext context)
    {
        context.Config.ReadyAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
    public Dictionary<string, string> GetParamComments(WorldContext context)
    {
        return context.Config.Data.ParamComments;
    }
}