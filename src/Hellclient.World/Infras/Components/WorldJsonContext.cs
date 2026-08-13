using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hellclient.World.Types;

namespace Hellclient.World.Infras.Components;

[JsonSerializable(typeof(Line))]
[JsonSerializable(typeof(List<Line>))]
[JsonSerializable(typeof(Word))]
[JsonSerializable(typeof(List<Trigger>))]
[JsonSerializable(typeof(List<Types.Timer>))]
[JsonSerializable(typeof(List<Alias>))]

public partial class WorldJsonContext : JsonSerializerContext
{
    public static JsonSerializerOptions JsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
    };
    public static JsonSerializerOptions JsonOptionsIndent = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
        WriteIndented = true
    };

    public static WorldJsonContext Instance = new WorldJsonContext(JsonOptions);
    public static WorldJsonContext InstanceIndent = new WorldJsonContext(JsonOptionsIndent);
    public static string Serialize(object? Data)
    {
        switch (Data)
        {
            case Line line:
                return JsonSerializer.Serialize(line, WorldJsonContext.Instance.Line);
            case List<Line> lineList:
                return JsonSerializer.Serialize(lineList, WorldJsonContext.Instance.ListLine);
            case Word word:
                return JsonSerializer.Serialize(word, WorldJsonContext.Instance.Word);
            case List<Trigger> triggerList:
                return JsonSerializer.Serialize(triggerList, WorldJsonContext.Instance.ListTrigger);
            case List<Types.Timer> timerList:
                return JsonSerializer.Serialize(timerList, WorldJsonContext.Instance.ListTimer);
            case List<Alias> aliasList:
                return JsonSerializer.Serialize(aliasList, WorldJsonContext.Instance.ListAlias);
            case null:
                return "null";
            default:
                throw new NotSupportedException($"Type {Data?.GetType()} is not supported for serialization.");
        }
    }
    public static string SerializeLineListIndented(List<Line> Data)
    {
        return JsonSerializer.Serialize(Data, WorldJsonContext.InstanceIndent.ListLine);
    }
}
