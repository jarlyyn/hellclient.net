using Hellclient.World.States;
using Hellclient.World.Types;
using Hellclient.World.Cores;
using Timer = Hellclient.World.Types.Timer;


namespace Hellclient.World.Cores;

public partial class World
{
    public string GetStatus() => string.Empty;
    public void SetStatus(string status) { }
    public int GetQueueDelay() => 0;
    public void SetQueueDelay(int queueDelay) { }
    public List<Command> GetQueue() => [];
    public string GetParam(string key) => string.Empty;
    public Dictionary<string, string> GetParams() => [];
    public void SetParam(string key, string value) { }
    public void DeleteParam(string key) { }
    public string GetParamComment(string key) => string.Empty;
    public Dictionary<string, string> GetParamComments() => [];
    public void SetParamComment(string key, string comment) { }
    public string GetCharset() => string.Empty;
    public long GetReadyAt() => 0L;
    public int GetPosition() => 0;
    public void SetPosition(int position) { }
    public WorldData? GetWorldData() => null;
    public ScriptData? GetScriptData() => null;
    public void SetPermissions(List<string> permissions) { }
    public List<string> GetPermissions() =>new();
    public void RequestPermissions(Authorization authorization) { }
    public string GetScriptID() => string.Empty;
    public void SetScriptID(string scriptID) { }
    public string GetScriptType() => string.Empty;
    public string GetScriptPath() => string.Empty;
    public string GetModPath() => string.Empty;
    public string GetCorePath() => string.Empty;
    public string GetScriptModPath() => string.Empty;
    public string GetLogsPath() => string.Empty;
    public string GetScriptHome() => string.Empty;
    public void DoLog(string message) { }
    public void SetTrusted(Trusted trusted) { }
    public Trusted? GetTrusted() => null;
    public void RequestTrustDomains(Authorization authorization) { }
    public object GetPluginOptions() => new object();
    public void DoReloadPermissions() { }
    public void DoSendToQueue(Command command) { }
    public void DoExecute(string message) { }
    public (byte[] Data, Exception? Error) DoEncode() => (Array.Empty<byte>(), null);
    public Exception? DoDecode(byte[] data) => null;
    public Exception? DoReloadScript() => null;
    public Exception? DoSaveScript() => null;
    public void DoUseScript(string scriptPath) { }
    public List<RequiredParam> GetRequiredParams() => [];
    public void DoRunScript(string script) { }

    public int DoDiscardQueue(bool force) => 0;
    public void DoLockQueue() { }
    public void DoSendHUDClickToScript(Click click) { }
    public void DoSendBroadcastToScript(Broadcast broadcast) { }
    public bool HandleBuffer(byte[] buffer) => false;
    public bool HandleSubneg(byte[] buffer) => false;
    public void HandleFocus() { }
    public void HandleLoseFocus() { }
    public void DoSendTimerToScript(Timer timer) { }
    public bool DoDeleteTimer(string id) => false;
    public bool DoDeleteTimerByName(string name) => false;
    public int DoDeleteTemporaryTimers() => 0;
    public int DoDeleteTimerGroup(string group, bool byUser) => 0;
    public bool DoEnableTimerByName(string name, bool enabled) => false;
    public int DoEnableTimerGroup(string group, bool enabled) => 0;
    public bool DoResetNamedTimer(string name) => false;
    public Timer? GetTimer(string name) => null;
    public List<Timer> GetTimersByType(bool byUser) => [];
    public void DoDeleteTimerByType(bool byUser) { }
    public void AddTimers(List<Timer> ts) { }
    public void DoResetTimers() { }
    public (string Value, bool Found, bool Valid) GetTimerOption(string name, string option) => (string.Empty, false, false);
    public (string Value, bool Found, bool Valid) GetTimerInfo(string name, int infotype) => (string.Empty, false, false);
    public (bool Updated, bool Found, bool Valid) SetTimerOption(string name, string option, string value) => (false, false, false);
    public bool HasNamedTimer(string name) => false;
    public List<string> DoListTimerNames(bool byUser) =>new();
    public bool AddTimer(Timer timer, bool byUser) => false;
    public int DoUpdateTimer(Timer timer) => 0;
    public void DoSendAliasToScript(string message, Alias alias, Matcher result) { }

    public bool DoDeleteAlias(string id) => false;
    public bool DoDeleteAliasByName(string name) => false;
    public int DoDeleteTemporaryAliases() => 0;
    public int DoDeleteAliasGroup(string group, bool byUser) => 0;
    public bool DoEnableAliasByName(string name, bool enabled) => false;
    public int DoEnableAliasGroup(string group, bool enabled) => 0;
    public Alias? GetAlias(string name) => null;
    public List<Alias> GetAliasesByType(bool byUser) => [];
    public void DoDeleteAliasByType(bool byUser) { }
    public void AddAliases(List<Alias> aliases) { }
    public (string Value, bool Found, bool Valid) GetAliasOption(string name, string option) => (string.Empty, false, false);
    public (string Value, bool Found, bool Valid) GetAliasInfo(string name, int infotype) => (string.Empty, false, false);
    public (bool Updated, bool Found, bool Valid) SetAliasOption(string name, string option, string value) => (false, false, false);
    public bool HasNamedAlias(string name) => false;
    public List<string> DoListAliasNames(bool byUser) =>new();
    public bool AddAlias(Alias alias, bool byUser) => false;
    public int DoUpdateAlias(Alias alias) => 0;

    public bool DoDeleteTrigger(string id) => false;
    public bool DoDeleteTriggerByName(string name) => false;
    public int DoDeleteTemporaryTriggers() => 0;
    public int DoDeleteTriggerGroup(string group, bool byUser) => 0;
    public bool DoEnableTriggerByName(string name, bool enabled) => false;
    public int DoEnableTriggerGroup(string group, bool enabled) => 0;
    public Trigger? GetTrigger(string name) => null;
    public List<Trigger> GetTriggersByType(bool byUser) => [];
    public void DoDeleteTriggerByType(bool byUser) { }
    public void AddTriggers(List<Trigger> triggers) { }
    public (string Value, bool Found, bool Valid) GetTriggerOption(string name, string option) => (string.Empty, false, false);
    public (string Value, bool Found, bool Valid) GetTriggerInfo(string name, int infotype) => (string.Empty, false, false);
    public (bool Updated, bool Found, bool Valid) SetTriggerOption(string name, string option, string value) => (false, false, false);
    public bool HasNamedTrigger(string name) => false;
    public List<string> DoListTriggerNames(bool byUser) =>new();
    public bool AddTrigger(Trigger trigger, bool byUser) => false;
    public int DoUpdateTrigger(Trigger trigger) => 0;
    public void DoSendTriggerToScript(Line line, Trigger trigger, Matcher result) { }
    public Matcher? DoGetTriggerWildcard(string name) => null;
    public void DoSendCallbackToScript(Callback cb) { }
    public void DoSendKeyUpToScript(string key) { }
    public void DoAssist() { }
    public void DoMultiLinesAppend(string line) { }
    public void DoMultiLinesFlush() { }
    public List<string> DoMultiLinesLast(int count) =>new();
    public IMapper? GetMapper() => null;
    public void HandleConnReceive(byte[] msg) { }
    public void HandleConnError(Exception err) { }
    public void HandleConnPrompt(byte[] msg) { }
    public void HandleConverterError(Exception err) { }
    public void HandleCmdError(Exception err) { }
    public void HandleTriggerError(Exception err) { }
    public void HandleScriptError(Exception err) { }
    public (string, string) GetScriptCaller() => (string.Empty, string.Empty);
    public void DoStopEvaluatingTriggers() { }

    public void DoSendResponseToScript(Message msg) { }

    public int GetHUDSize() => 0;
    public void SetHUDSize(int size) { }
    public List<Line> GetHUDContent() => [];
    public bool UpdateHUDContent(int start, List<Line> content) => false;
}