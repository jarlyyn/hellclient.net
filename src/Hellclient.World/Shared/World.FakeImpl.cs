using Hellclient.World.Contexts;
using Hellclient.World.Models;
using Hellclient.Mapper.Shared;
using Timer = Hellclient.World.Models.Timer;


namespace Hellclient.World.Shared;

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
    public void SetCharset(string charset) { }
    public long GetReadyAt() => 0L;
    public int GetPosition() => 0;
    public void SetPosition(int position) { }
    public List<Line> GetCurrentLines() => [];
    public Line? GetPrompt() => null;
    public ClientInfo? GetClientInfo() => null;
    public WorldData? GetWorldData() => null;
    public ScriptData? GetScriptData() => null;
    public void SetPermissions(string[] permissions) { }
    public string[] GetPermissions() => Array.Empty<string>();
    public void RequestPermissions(Authorization authorization) { }
    public string GetScriptID() => string.Empty;
    public void SetScriptID(string scriptID) { }
    public string GetScriptType() => string.Empty;
    public string GetScriptPath() => string.Empty;
    public string GetModPath() => string.Empty;
    public string GetSharedPath() => string.Empty;
    public string GetScriptModPath() => string.Empty;
    public string GetLogsPath() => string.Empty;
    public string GetScriptHome() => string.Empty;
    public void DoLog(string message) { }
    public void SetTrusted(Trusted trusted) { }
    public Trusted? GetTrusted() => null;
    public void RequestTrustDomains(Authorization authorization) { }
    public object GetPluginOptions() => new object();
    public void DoReloadPermissions() { }
    public void DoSend(Command command) { }
    public void DoSendToQueue(Command command) { }
    public void DoExecute(string message) { }
    public (byte[] Data, Exception? Error) DoEncode() => (Array.Empty<byte>(), null);
    public Exception? DoDecode(byte[] data) => null;
    public Exception? DoReloadScript() => null;
    public Exception? DoSaveScript() => null;
    public void DoUseScript(string scriptPath) { }
    public List<RequiredParam> GetRequiredParams() => [];
    public void DoRunScript(string script) { }
    public void DoPrint(string msg) { }
    public void DoPrintSystem(string msg) { }
    public void DoPrintLocalBroadcastIn(string msg) { }
    public void DoPrintGlobalBroadcastIn(string msg) { }
    public void DoPrintLocalBroadcastOut(string msg) { }
    public void DoPrintGlobalBroadcastOut(string msg) { }
    public void DoPrintSubneg(string msg) { }
    public void DoPrintRequest(string msg) { }
    public void DoPrintResponse(string msg) { }

    public int DoDiscardQueue(bool force) => 0;
    public void DoLockQueue() { }
    public void DoOmitOutput() { }
    public void DoDeleteLines(int count) { }
    public int GetLineCount() => 0;
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
    public string[] DoListTimerNames(bool byUser) => Array.Empty<string>();
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
    public string[] DoListAliasNames(bool byUser) => Array.Empty<string>();
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
    public string[] DoListTriggerNames(bool byUser) => Array.Empty<string>();
    public bool AddTrigger(Trigger trigger, bool byUser) => false;
    public int DoUpdateTrigger(Trigger trigger) => 0;
    public void DoSendTriggerToScript(Line line, Trigger trigger, Matcher result) { }
    public Matcher? DoGetTriggerWildcard(string name) => null;
    public void DoSendCallbackToScript(Callback cb) { }
    public void DoSendKeyUpToScript(string key) { }
    public void DoAssist() { }
    public void DoMultiLinesAppend(string line) { }
    public void DoMultiLinesFlush() { }
    public string[] DoMultiLinesLast(int count) => Array.Empty<string>();
    public int GetLinesInBufferCount() => 0;
    public List<Line> GetRecentLines(int count) => [];
    public Line? GetLine(int idx) => null;
    public IMapper? GetMapper() => null;
    public int GetPriority() => 0;
    public void SetPriority(int priority) { }
    public List<Line> GetSummary() => [];
    public void SetSummary(List<Line> lines) { }
    public void UpdateLastActive() { }
    public long GetLastActive() => 0L;
    public void AddHistory(string value) { }
    public string[] GetHistories() => Array.Empty<string>();
    public void FlushHistories() { }
    public void HandleConnReceive(byte[] msg) { }
    public void HandleConnError(Exception err) { }
    public void HandleConnPrompt(byte[] msg) { }
    public void HandleConverterError(Exception err) { }
    public void HandleCmdError(Exception err) { }
    public void HandleTriggerError(Exception err) { }
    public void HandleScriptError(Exception err) { }
    public (string, string) GetScriptCaller() => (string.Empty, string.Empty);
    public void DoStopEvaluatingTriggers() { }

    public int GetMetronomeBeats() => 0;
    public void SetMetronomeBeats(int beats) { }
    public void DoResetMetronome() { }
    public int GetMetronomeSpace() => 0;
    public string[] GetMetronomeQueue() => Array.Empty<string>();
    public bool DoDiscardMetronome(bool force) => false;
    public void DoLockMetronomeQueue() { }
    public void DoFullMetronome() { }
    public void DoFullTickMetronome() { }
    public void SetMetronomeInterval(TimeSpan interval) { }
    public TimeSpan GetMetronomeInterval() => TimeSpan.Zero;
    public void SetMetronomeTick(TimeSpan tick) { }
    public TimeSpan GetMetronomeTick() => TimeSpan.Zero;
    public void DoPushMetronome(List<Command> cmds, bool grouped) { }
    public void DoMetronomeSend(Command cmds) { }
    public void DoMetronomeLock() { }
    public void DoSendResponseToScript(Message msg) { }

    public int GetHUDSize() => 0;
    public void SetHUDSize(int size) { }
    public List<Line> GetHUDContent() => [];
    public bool UpdateHUDContent(int start, List<Line> content) => false;
}