namespace Hellclient.Core.Types.Forms;

public class UpdateTriggerForm
{
    public string World { get; set; } = "";
    public bool ByUser { get; set; }
    public string ID { get; set; } = "";
    public string Name { get; set; } = "";
    public bool Enabled { get; set; }
    public string Match { get; set; } = "";
    public string Send { get; set; } = "";
    public string Script { get; set; } = "";
    public int SendTo { get; set; }
    public int Sequence { get; set; }
    public bool ExpandVariables { get; set; }
    public bool Temporary { get; set; }
    public bool OneShot { get; set; }
    public bool Regexp { get; set; }
    public string Group { get; set; } = "";
    public string Variable { get; set; } = "";
    public bool IgnoreCase { get; set; }
    public bool KeepEvaluating { get; set; }
    public bool OmitFromLog { get; set; }
    public bool OmitFromOutput { get; set; }
    public bool MultiLine { get; set; }
    public bool Repeat { get; set; }
    public int LinesToMatch { get; set; }
    public bool WildcardLowerCase { get; set; }
  
}