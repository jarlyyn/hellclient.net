using Hellclient.World.Infras.Adapters;
using Hellclient.World.Types;

namespace Hellclient.World.Infras.Components;

public class Script
{
    private CreatorInfo CreatorInfo = new("", "");
    public string Status { get; set; } = string.Empty;
    public ScriptData Data = new ScriptData();
    public IScriptEngine Engine = new NopScriptEngine();
    public PlainOptions Options = new PlainOptions();
    public void SetCreator(string creator, string creatorType)
    {
        this.CreatorInfo = new CreatorInfo(creator, creatorType);
    }
    public CreatorInfo CreatorAndType() => this.CreatorInfo;

    public void Reset()
    {
        this.Data = new ScriptData();
        this.Status = string.Empty;
        this.Engine = new NopScriptEngine();
    }
    public bool CanRun() => this.Engine is not NopScriptEngine;
}