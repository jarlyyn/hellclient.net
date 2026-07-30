using Hellclient.World.Infras.Components;
using Hellclient.World.Types;
using Tomlyn;

namespace Hellclient.World.Features.Repo;

public interface IScriptFileRepo
{
    public void SaveScriptData(ScriptData data, string id);
    public ScriptData LoadScriptData(string id);
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

    public void SaveScriptData(ScriptData data, string id)
    {
    }
    public ScriptData LoadScriptData(string id)
    {
        return new ScriptData();
    }
}