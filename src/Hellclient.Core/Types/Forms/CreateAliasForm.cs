namespace Hellclient.Core.Types.Forms;

public class CreateAliasForm
{
    public string World { get; set; } = "";
    public bool ByUser { get; set; } = false;
    public string ID { get; set; } = "";
    public string Name { get; set; } = "";
    public bool Enabled { get; set; } = false;
    public string Match { get; set; } = "";
    public string Send { get; set; } = "";
    public string Script { get; set; } = "";
    public int SendTo { get; set; } = 0;
    public int Sequence { get; set; } = 0;
    public bool ExpandVariables { get; set; } = false;
    public bool Temporary { get; set; } = false;
    public bool OneShot { get; set; } = false;
    public bool Regexp { get; set; } = false;
    public string Group { get; set; } = "";
    public string Variable { get; set; } = "";
    public bool IgnoreCase { get; set; } = false;
    public bool KeepEvaluating { get; set; } = false;
    public bool Menu { get; set; } = false;
    public bool OmitFromLog { get; set; } = false;
    public bool ReverseSpeedwalk { get; set; } = false;
    public bool OmitFromOutput { get; set; } = false;
}
