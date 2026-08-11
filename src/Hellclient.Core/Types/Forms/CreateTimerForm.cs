namespace Hellclient.Core.Types.Forms;

public class CreateTimerForm
{
    public string World { get; set; } = "";
    public bool ByUser { get; set; }
    public int Hour { get; set; }
    public int Minute { get; set; }
    public double Second { get; set; }
    public string Name { get; set; } = "";
    public int SendTo { get; set; }
    public string Send { get; set; } = "";
    public string Script { get; set; } = "";
    public string Group { get; set; } = "";
    public string Variable { get; set; } = "";
    public bool AtTime { get; set; }
    public bool Enabled { get; set; }
    public bool ActionWhenDisconnectd { get; set; }
    public bool OneShot { get; set; }
    public bool Temporary { get; set; }
    public bool OmitFromOutput { get; set; }
    public bool OmitFromLog { get; set; }
}