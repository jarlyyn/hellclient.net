using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hellclient.Core.Types;
using Hellclient.Core.Types.Forms;
using Hellclient.World.Types;
using Timer = Hellclient.World.Types.Timer;

namespace Hellclient.Core.Infras.Components;

[JsonSerializable(typeof(BatchCommand))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(List<string>))]
[JsonSerializable(typeof(Callback))]
[JsonSerializable(typeof(Authorization))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(Click))]
[JsonSerializable(typeof(BatchCommand))]
[JsonSerializable(typeof(Hellclient.Core.Types.Message))]
[JsonSerializable(typeof(DateVersion))]
[JsonSerializable(typeof(ClientInfo))]
[JsonSerializable(typeof(List<ClientInfo>))]
[JsonSerializable(typeof(WorldFile))]
[JsonSerializable(typeof(List<WorldFile>))]
[JsonSerializable(typeof(ParamsInfo))]
[JsonSerializable(typeof(Line))]
[JsonSerializable(typeof(List<Line>))]
[JsonSerializable(typeof(FieldError))]
[JsonSerializable(typeof(List<FieldError>))]
[JsonSerializable(typeof(ScriptInfo))]
[JsonSerializable(typeof(List<ScriptInfo>))]
[JsonSerializable(typeof(Timer))]
[JsonSerializable(typeof(List<Timer>))]
[JsonSerializable(typeof(Alias))]
[JsonSerializable(typeof(List<Alias>))]
[JsonSerializable(typeof(Trigger))]
[JsonSerializable(typeof(List<Trigger>))]
[JsonSerializable(typeof(WorldSettings))]
[JsonSerializable(typeof(ScriptSettings))]
[JsonSerializable(typeof(RequiredParam))]
[JsonSerializable(typeof(List<RequiredParam>))]
[JsonSerializable(typeof(Authorized))]
[JsonSerializable(typeof(FoundHistory))]
[JsonSerializable(typeof(DiffLines))]
[JsonSerializable(typeof(BatchCommandScripts))]
[JsonSerializable(typeof(CreateAliasForm))]
[JsonSerializable(typeof(CreateGameForm))]
[JsonSerializable(typeof(CreateScriptForm))]
[JsonSerializable(typeof(CreateTimerForm))]
[JsonSerializable(typeof(CreateTriggerForm))]
[JsonSerializable(typeof(RequiredParamsForm))]

public partial class JsonContext : JsonSerializerContext
{
    public static JsonSerializerOptions JsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };
    public static JsonContext Instance = new JsonContext(JsonOptions);
    public static byte[] Serialize(object? Data)
    {
        switch (Data)
        {
            case BatchCommand batchCommand:
                return JsonSerializer.SerializeToUtf8Bytes(batchCommand, JsonContext.Instance.BatchCommand);
            case string str:
                return JsonSerializer.SerializeToUtf8Bytes(str, JsonContext.Instance.String);
            case List<string> strList:
                return JsonSerializer.SerializeToUtf8Bytes(strList, JsonContext.Instance.ListString);
            case Callback callback:
                return JsonSerializer.SerializeToUtf8Bytes(callback, JsonContext.Instance.Callback);
            case Authorization auth:
                return JsonSerializer.SerializeToUtf8Bytes(auth, JsonContext.Instance.Authorization);
            case int i:
                return JsonSerializer.SerializeToUtf8Bytes(i, JsonContext.Instance.Int32);
            case Click click:
                return JsonSerializer.SerializeToUtf8Bytes(click, JsonContext.Instance.Click);
            case Hellclient.Core.Types.Message msg:
                return JsonSerializer.SerializeToUtf8Bytes(msg, JsonContext.Instance.Message);
            case DateVersion dv:
                return JsonSerializer.SerializeToUtf8Bytes(dv, JsonContext.Instance.DateVersion);
            case ClientInfo ci:
                return JsonSerializer.SerializeToUtf8Bytes(ci, JsonContext.Instance.ClientInfo);
            case List<ClientInfo> ciList:
                return JsonSerializer.SerializeToUtf8Bytes(ciList, JsonContext.Instance.ListClientInfo);
            case WorldFile wf:
                return JsonSerializer.SerializeToUtf8Bytes(wf, JsonContext.Instance.WorldFile);
            case List<WorldFile> wfList:
                return JsonSerializer.SerializeToUtf8Bytes(wfList, JsonContext.Instance.ListWorldFile);
            case ParamsInfo pi:
                return JsonSerializer.SerializeToUtf8Bytes(pi, JsonContext.Instance.ParamsInfo);
            case Line line:
                return JsonSerializer.SerializeToUtf8Bytes(line, JsonContext.Instance.Line);
            case List<Line> lineList:
                return JsonSerializer.SerializeToUtf8Bytes(lineList, JsonContext.Instance.ListLine);
            case FieldError fe:
                return JsonSerializer.SerializeToUtf8Bytes(fe, JsonContext.Instance.FieldError);
            case List<FieldError> feList:
                return JsonSerializer.SerializeToUtf8Bytes(feList, JsonContext.Instance.ListFieldError);
            case ScriptInfo si:
                return JsonSerializer.SerializeToUtf8Bytes(si, JsonContext.Instance.ScriptInfo);
            case List<ScriptInfo> siList:
                return JsonSerializer.SerializeToUtf8Bytes(siList, JsonContext.Instance.ListScriptInfo);
            case Timer timer:
                return JsonSerializer.SerializeToUtf8Bytes(timer, JsonContext.Instance.Timer);
            case List<Timer> timerList:
                return JsonSerializer.SerializeToUtf8Bytes(timerList, JsonContext.Instance.ListTimer);
            case Alias alias:
                return JsonSerializer.SerializeToUtf8Bytes(alias, JsonContext.Instance.Alias);
            case List<Alias> aliasList:
                return JsonSerializer.SerializeToUtf8Bytes(aliasList, JsonContext.Instance.ListAlias);
            case Trigger trigger:
                return JsonSerializer.SerializeToUtf8Bytes(trigger, JsonContext.Instance.Trigger);
            case List<Trigger> triggerList:
                return JsonSerializer.SerializeToUtf8Bytes(triggerList, JsonContext.Instance.ListTrigger);
            case WorldSettings ws:
                return JsonSerializer.SerializeToUtf8Bytes(ws, JsonContext.Instance.WorldSettings);
            case ScriptSettings ss:
                return JsonSerializer.SerializeToUtf8Bytes(ss, JsonContext.Instance.ScriptSettings);
            case RequiredParam rp:
                return JsonSerializer.SerializeToUtf8Bytes(rp, JsonContext.Instance.RequiredParam);
            case List<RequiredParam> rpList:
                return JsonSerializer.SerializeToUtf8Bytes(rpList, JsonContext.Instance.ListRequiredParam);
            case Authorized authz:
                return JsonSerializer.SerializeToUtf8Bytes(authz, JsonContext.Instance.Authorized);
            case FoundHistory fh:
                return JsonSerializer.SerializeToUtf8Bytes(fh, JsonContext.Instance.FoundHistory);
            case DiffLines dl:
                return JsonSerializer.SerializeToUtf8Bytes(dl, JsonContext.Instance.DiffLines);
            case BatchCommandScripts bcs:
                return JsonSerializer.SerializeToUtf8Bytes(bcs, JsonContext.Instance.BatchCommandScripts);
            case CreateAliasForm caf:
                return JsonSerializer.SerializeToUtf8Bytes(caf, JsonContext.Instance.CreateAliasForm);
            case CreateGameForm cgf:
                return JsonSerializer.SerializeToUtf8Bytes(cgf, JsonContext.Instance.CreateGameForm);
            case CreateScriptForm csf:
                return JsonSerializer.SerializeToUtf8Bytes(csf, JsonContext.Instance.CreateScriptForm);
            case CreateTimerForm ctf:
                return JsonSerializer.SerializeToUtf8Bytes(ctf, JsonContext.Instance.CreateTimerForm);
            case RequiredParamsForm rpf:
                return JsonSerializer.SerializeToUtf8Bytes(rpf, JsonContext.Instance.RequiredParamsForm);
            case CreateTriggerForm ctf:
                return JsonSerializer.SerializeToUtf8Bytes(ctf, JsonContext.Instance.CreateTriggerForm);
            case null:
                return Encoding.UTF8.GetBytes("null");
            default:
                throw new NotSupportedException($"Type {Data?.GetType()} is not supported for serialization.");
        }
    }
}

