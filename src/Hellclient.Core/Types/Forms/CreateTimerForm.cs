namespace Hellclient.Core.Types.Forms;

public class CreateTimerForm
{
    public string World { get; set; } = "";
    public bool ByUser { get; set; }
    public int Hour { get; set; } = 0;
    public int Minute { get; set; } = 0;
    public double Second { get; set; } = 0;
    public string Name { get; set; } = "";
    public int SendTo { get; set; } = 0;
    public string Send { get; set; } = "";
    public string Script { get; set; } = "";
    public string Group { get; set; } = "";
    public string Variable { get; set; } = "";
    public bool AtTime { get; set; } = false;
    public bool Enabled { get; set; } = false;
    public bool ActionWhenDisconnectd { get; set; } = false;
    public bool OneShot { get; set; } = false;
    public bool Temporary { get; set; } = false;
    public bool OmitFromOutput { get; set; } = false;
    public bool OmitFromLog { get; set; } = false;
}