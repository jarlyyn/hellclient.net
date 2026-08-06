using Hellclient.Core.Types;
using Hellclient.World.Types;
using Message = Hellclient.Core.Types.Message;
using Timer = Hellclient.World.Types.Timer;

namespace Hellclient.Core.Helpers;
public class MsgHelper
{
    public const string MsgTypeConnected = "connected";
    public const string MsgTypeDisconnected = "disconnected";
    public const string MsgTypeCreateFail = "createFail";
    public const string MsgTypeCreateSuccess = "createSuccess";
    public const string MsgTypeUpdateSuccess = "updateSuccess";
    public const string MsgTypeCreateScriptFail = "createScriptFail";
    public const string MsgTypeCreateScriptSuccess = "createScriptSuccess";
    public const string MsgTypeUpdateScriptSuccess = "updateScriptSuccess";
    public const string MsgTypeLine = "line";
    public const string MsgTypePrompt = "prompt";
    public const string MsgTypeAllLines = "allLines";
    public const string MsgTypeLines = "lines";
    public const string MsgTypeClients = "clients";
    public const string MsgTypeNotOpened = "notopened";
    public const string MsgTypeScriptInfo = "scriptinfo";
    public const string MsgTypeScriptInfoList = "scriptinfoList";
    public const string MsgTypeStatus = "status";
    public const string MsgTypeHistory = "history";
    public const string MsgTypeUserTimers = "usertimers";
    public const string MsgTypeScriptTimers = "scripttimers";
    public const string MsgTypeCreateTimerSuccess = "createTimerSuccess";
    public const string MsgTypeTimer = "timer";
    public const string MsgTypeUpdateTimerSuccess = "updateTimerSuccess";
    public const string MsgTypeUserAliases = "useraliases";
    public const string MsgTypeScriptAliases = "scriptaliases";
    public const string MsgTypeCreateAliasSuccess = "createAliasSuccess";
    public const string MsgTypeAlias = "alias";
    public const string MsgTypeUpdateAliasSuccess = "updateAliasSuccess";
    public const string MsgTypeUserTriggers = "usertriggers";
    public const string MsgTypeScriptTriggers = "scripttriggers";
    public const string MsgTypeCreateTriggerSuccess = "createTriggerSuccess";
    public const string MsgTypeTrigger = "trigger";
    public const string MsgTypeUpdateTriggerSuccess = "updateTriggerSuccess";
    public const string MsgTypeParamsinfo = "paramsinfo";
    public const string MsgTypeParamUpdated = "paramupdated";
    public const string MsgTypeParamDeleted = "paramdeleted";
    public const string MsgTypeParamCommentUpdated = "paramcommentupdated";
    public const string MsgTypeScriptMessage = "scriptMessage";
    public const string MsgTypeSwitchStatusMessage = "switchStatus";
    public const string MsgTypeVersionMessage = "version";
    public const string MsgTypeAPIVersionMessage = "apiversion";
    public const string MsgTypeWorldSettingsMessage = "worldSettings";
    public const string MsgTypeScriptSettingsMessage = "scriptSettings";
    public const string MsgTypeRequiredParamsMessage = "requiredParams";
    public const string MsgTypeDefaultServer = "defaultServer";
    public const string MsgTypeDefaultCharset = "defaultCharset";
    public const string MsgTypeRequestPermissions = "requestPermissions";
    public const string MsgTypeRequestTrustDomains = "requestTrustDomains";
    public const string MsgTypeAuthorized = "authorized";
    public const string MsgTypeFoundHistory = "foundhistory";
    public const string MsgTypeHUDUpdate = "hudupdate";
    public const string MsgTypeHUDContent = "hudcontent";
    public const string MsgTypeClientInfo = "clientinfo";
    public const string MsgTypeBatchCommandScripts = "batchcommandscripts";
        public static void PublishConnected(IPublisher p, string id)
        {
            p.Publish(new Message(MsgTypeConnected, "", id));
        }

        public static void PublishDisconnected(IPublisher p, string id)
        {
            p.Publish(new Message(MsgTypeDisconnected, "", id));
        }

        public static void PublishCreateFail(IPublisher p, List<FieldError> errors)
        {
            p.Publish(new Message(MsgTypeCreateFail, "", errors));
        }

        public static void PublishCreateSuccess(IPublisher p, string id)
        {
            p.Publish(new Message(MsgTypeCreateSuccess, "", id));
        }

        public static void PublishUpdateScriptSuccess(IPublisher p, string id)
        {
            p.Publish(new Message(MsgTypeUpdateScriptSuccess, "", id));
        }

        public static void PublishUpdateSuccess(IPublisher p, string id)
        {
            p.Publish(new Message(MsgTypeUpdateSuccess, "", id));
        }

        public static void PublishCreateScriptFail(IPublisher p, List<FieldError> errors)
        {
            p.Publish(new Message(MsgTypeCreateScriptFail, "", errors));
        }

        public static void PublishCreateScriptSuccess(IPublisher p, string id)
        {
            p.Publish(new Message(MsgTypeCreateScriptSuccess, "", id));
        }

        public static void PublishLine(IPublisher p, string id, Line line)
        {
            p.Publish(new Message(MsgTypeLine, id, line));
        }

        public static void PublishPrompt(IPublisher p, string id, Line? prompt)
        {
            p.Publish(new Message(MsgTypePrompt, id, prompt));
        }

        public static void PublishAllLines(IPublisher p, string id, List<Line> lines)
        {
            p.Publish(new Message(MsgTypeAllLines, id, lines));
        }

        public static void PublishLines(IPublisher p, string id, List<Line> lines)
        {
            p.Publish(new Message(MsgTypeLines, id, lines));
        }

        public static void PublishClients(IPublisher p, List<ClientInfo> infos)
        {
            p.Publish(new Message(MsgTypeClients, "", infos));
        }

        public static void PublishNotOpened(IPublisher p, List<WorldFile> list)
        {
            p.Publish(new Message(MsgTypeNotOpened, "", list));
        }

        public static void PublishScriptInfo(IPublisher p, string id, ScriptInfo info)
        {
            p.Publish(new Message(MsgTypeScriptInfo, id, info));
        }

        public static void PublishScriptInfoList(IPublisher p, List<ScriptInfo> info)
        {
            p.Publish(new Message(MsgTypeScriptInfoList, "", info));
        }

        public static void PublishStatus(IPublisher p, string id, string status)
        {
            p.Publish(new Message(MsgTypeStatus, id, status));
        }

        public static void PublishHistory(IPublisher p, string id, List<string> history)
        {
            p.Publish(new Message(MsgTypeHistory, id, history));
        }

        public static void PublishUserTimers(IPublisher p, string id, List<Timer> timers)
        {
            p.Publish(new Message(MsgTypeUserTimers, id, timers));
        }

        public static void PublishScriptTimers(IPublisher p, string id, List<Timer> timers)
        {
            p.Publish(new Message(MsgTypeScriptTimers, id, timers));
        }

        public static void PublishCreateTimerSuccess(IPublisher p, string world, string id)
        {
            p.Publish(new Message(MsgTypeCreateTimerSuccess, world, id));
        }

        public static void PublishTimer(IPublisher p, string world, Timer timer)
        {
            p.Publish(new Message(MsgTypeTimer, world, timer));
        }

        public static void PublishUpdateTimerSuccess(IPublisher p, string world, string id)
        {
            p.Publish(new Message(MsgTypeUpdateTimerSuccess, world, id));
        }

        public static void PublishUserAliases(IPublisher p, string id, List<Alias> aliases)
        {
            p.Publish(new Message(MsgTypeUserAliases, id, aliases));
        }

        public static void PublishScriptAliases(IPublisher p, string id, List<Alias> aliases)
        {
            p.Publish(new Message(MsgTypeScriptAliases, id, aliases));
        }

        public static void PublishCreateAliasSuccess(IPublisher p, string world, string id)
        {
            p.Publish(new Message(MsgTypeCreateAliasSuccess, world, id));
        }

        public static void PublishAlias(IPublisher p, string world, Alias alias)
        {
            p.Publish(new Message(MsgTypeAlias, world, alias));
        }

        public static void PublishUpdateAliasSuccess(IPublisher p, string world, string id)
        {
            p.Publish(new Message(MsgTypeUpdateAliasSuccess, world, id));
        }

        public static void PublishUserTriggers(IPublisher p, string id, List<Trigger> triggers)
        {
            p.Publish(new Message(MsgTypeUserTriggers, id, triggers));
        }

        public static void PublishScriptTriggers(IPublisher p, string id, List<Trigger> triggers)
        {
            p.Publish(new Message(MsgTypeScriptTriggers, id, triggers));
        }

        public static void PublishCreateTriggerSuccess(IPublisher p, string world, string id)
        {
            p.Publish(new Message(MsgTypeCreateTriggerSuccess, world, id));
        }

        public static void PublishTrigger(IPublisher p, string world, Trigger trigger)
        {
            p.Publish(new Message(MsgTypeTrigger, world, trigger));
        }

        public static void PublishUpdateTriggerSuccess(IPublisher p, string world, string id)
        {
            p.Publish(new Message(MsgTypeUpdateTriggerSuccess, world, id));
        }

        public static void PublishParamsinfo(IPublisher p, string world, ParamsInfo info)
        {
            p.Publish(new Message(MsgTypeParamsinfo, world, info));
        }

        public static void PublishParamUpdated(IPublisher p, string world, string name)
        {
            p.Publish(new Message(MsgTypeParamUpdated, world, name));
        }

        public static void PublishParamCommentUpdated(IPublisher p, string world, string name)
        {
            p.Publish(new Message(MsgTypeParamCommentUpdated, world, name));
        }

        public static void PublishParamDeleted(IPublisher p, string world, string name)
        {
            p.Publish(new Message(MsgTypeParamDeleted, world, name));
        }

        public static void PublishScriptMessage(IPublisher p, string world, object msg)
        {
            p.Publish(new Message(MsgTypeScriptMessage, world, msg));
        }

        public static void PublishSwitchStatusMessage(IPublisher p, int status)
        {
            p.Publish(new Message(MsgTypeSwitchStatusMessage, "", status.ToString()));
        }

        public static void PublishVersionMessage(IPublisher p, string version)
        {
            p.Publish(new Message(MsgTypeVersionMessage, "", version));
        }

        public static void PublishAPIVersionMessage(IPublisher p, DateVersion version)
        {
            p.Publish(new Message(MsgTypeAPIVersionMessage, "", version));
        }

        public static void PublishWorldSettingsMessage(IPublisher p, string world, WorldSettings settings)
        {
            p.Publish(new Message(MsgTypeWorldSettingsMessage, world, settings));
        }

        public static void PublishScriptSettingsMessage(IPublisher p, string world, ScriptSettings settings)
        {
            p.Publish(new Message(MsgTypeScriptSettingsMessage, world, settings));
        }

        public static void PublishRequiredParamsMessage(IPublisher p, string world, List<RequiredParam> rp)
        {
            p.Publish(new Message(MsgTypeRequiredParamsMessage, world, rp));
        }

        public static void PublishDefaultServerMessage(IPublisher p, string server)
        {
            p.Publish(new Message(MsgTypeDefaultServer, "", server));
        }

        public static void PublishDefaultCharsetMessage(IPublisher p, string charset)
        {
            p.Publish(new Message(MsgTypeDefaultCharset, "", charset));
        }

        public static void PublishRequestPermissions(IPublisher p, string world, Authorization a)
        {
            p.Publish(new Message(MsgTypeRequestPermissions, world, a));
        }

        public static void PublishRequestTrustDomains(IPublisher p, string world, Authorization a)
        {
            p.Publish(new Message(MsgTypeRequestTrustDomains, world, a));
        }

        public static void PublishAuthorized(IPublisher p, string world, Authorized a)
        {
            p.Publish(new Message(MsgTypeAuthorized, world, a));
        }

        public static void PublishFoundHistory(IPublisher p, string world, FoundHistory h)
        {
            p.Publish(new Message(MsgTypeFoundHistory, world, h));
        }

        public static void PublishHUDUpdate(IPublisher p, string world, DiffLines diff)
        {
            p.Publish(new Message(MsgTypeHUDUpdate, world, diff));
        }

        public static void PublishHUDContent(IPublisher p, string world, List<Line> lines)
        {
            p.Publish(new Message(MsgTypeHUDContent, world, lines));
        }

        public static void PublishClientInfo(IPublisher p, string world, ClientInfo info)
        {
            p.Publish(new Message(MsgTypeClientInfo, world, info));
        }

        public static void PublishBatchCommandScripts(IPublisher p, BatchCommandScripts scripts)
        {
            p.Publish(new Message(MsgTypeBatchCommandScripts, "", scripts));
        }

}
