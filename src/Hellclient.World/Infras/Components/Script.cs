using Hellclient.World.Infras.Adapters;
using Hellclient.World.Types;

namespace Hellclient.World.Infras.Components;

public class Script
{
    private CreatorInfo CreatorInfo = new("", "");
    public string Status { get; set; } = string.Empty;
    public ScriptData? Data = null;
    public IScriptEngine Engine = new NopScriptEngine();
    public void SetCreator(string creator, string creatorType)
    {
        this.CreatorInfo = new CreatorInfo(creator, creatorType);
    }
    public CreatorInfo CreatorAndType() => this.CreatorInfo;

}