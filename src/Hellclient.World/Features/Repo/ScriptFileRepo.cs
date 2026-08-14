using Hellclient.World.Infras.Components;
using Hellclient.World.States;
using Hellclient.World.Types;
using Tomlyn;

namespace Hellclient.World.Features.Repo;

public interface IScriptFileRepo
{
    public void SaveScriptData(WorldContext context, ScriptData data, string id);
    public ScriptData LoadScriptData(WorldContext context, string id);

}
public class ScriptFileRepo : IScriptFileRepo
{
    private string encodeScript(ScriptData data)
    {
        var scriptData = TomlSerializer.Serialize<ScriptData>(data, TomlContext.Default.ScriptData);
        return scriptData;
    }
    private ScriptData decode(string data)
    {
        var scriptdata = TomlSerializer.Deserialize<ScriptData>(data, TomlContext.Default.ScriptData);
        return scriptdata!;
    }

    public void SaveScriptData(WorldContext context, ScriptData data, string id)
    {
        var path = Path.Combine(context.Paths.ScriptPath, id, "script.toml");
        var scriptData = encodeScript(data);
        System.IO.File.WriteAllText(path, scriptData);
    }
    public ScriptData LoadScriptData(WorldContext context, string id)
    {
        var path = Path.Combine(context.Paths.ScriptPath, id, "script.toml");
        var data=System.IO.File.ReadAllText(path);
        var scriptData = decode(data);
        return scriptData;
    }
}