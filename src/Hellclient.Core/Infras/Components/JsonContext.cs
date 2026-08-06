using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hellclient.World.Types;

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
            case null:
                return Encoding.UTF8.GetBytes("\"null\"");
            default:
                throw new NotSupportedException($"Type {Data?.GetType()} is not supported for serialization.");
        }
    }
}
