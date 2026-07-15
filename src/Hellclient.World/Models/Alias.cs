namespace Hellclient.World.Models;

public class Alias
{
    public const int AliasFlagEnabled = 1;
    public const int AliasFlagKeepEvaluating = 8;
    public const int AliasFlagIgnoreAliasCase = 32;
    public const int AliasFlagOmitFromLogFile = 64;
    public const int AliasFlagRegularExpression = 128;
    public const int AliasFlagExpandVariables = 512;
    public const int AliasFlagReplace = 1024;
    public const int AliasFlagAliasSpeedWalk = 2048;
    public const int AliasFlagAliasQueue = 4096;
    public const int AliasFlagAliasMenu = 8192;
    public const int AliasFlagTemporary = 16384;
    public string ID { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool Enabled { get; set; } = false;
    public string Match { get; set; } = string.Empty;
    public string Send { get; set; } = string.Empty;
    public string Script { get; set; } = string.Empty;
    public int SendTo { get; set; } = 0;
    public int Sequence { get; set; } = 0;
    public bool ExpandVariables { get; set; } = false;
    public bool Temporary { get; set; } = false;
    public bool OneShot { get; set; } = false;
    public bool Regexp { get; set; } = false;
    public string Group { get; set; } = string.Empty;
    public bool IgnoreCase { get; set; } = false;
    public bool KeepEvaluating { get; set; } = false;
    public bool Menu { get; set; } = false;
    public bool OmitFromLog { get; set; } = false;
    public string Variable { get; set; } = string.Empty;
    public bool ReverseSpeedwalk { get; set; } = false;
    public bool OmitFromOutput { get; set; } = false;
    public bool OmitFromCommandHistory { get; set; } = false;
    private bool byuser = false;
}