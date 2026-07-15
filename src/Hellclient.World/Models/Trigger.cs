namespace Hellclient.World.Models;

public class Trigger
{
    public const int TriggerFlagEnabled = 1;
    public const int TriggerFlagOmitFromLog = 2;
    public const int TriggerFlagOmitFromOutput = 4;
    public const int TriggerFlagKeepEvaluating = 8;
    public const int TriggerFlagIgnoreCase = 16;
    public const int TriggerFlagRegularExpression = 32;
    public const int TriggerFlagExpandVariables = 512;
    public const int TriggerFlagReplace = 1024;
    public const int TriggerFlagTemporary = 16384;
    public const int TriggerFlagLowercaseWildcard = 2048;
    public const int TriggerFlagOneShot = 32768;
    public string ID { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool Enabled { get; set; } = false;
    public string Match { get; set; } = string.Empty;
    public string Send { get; set; } = string.Empty;
    public int ColourChangeType { get; set; } = 0;
    public int Colour { get; set; } = 0;
    public int Wildcard { get; set; } = 0;
    public string SoundFileName { get; set; } = string.Empty;
    public bool SoundIfInactive { get; set; } = false;
    public string Script { get; set; } = string.Empty;
    public int SendTo { get; set; } = 0;
    public int Sequence { get; set; } = 0;
    public bool ExpandVariables { get; set; } = false;
    public bool Temporary { get; set; } = false;
    public bool OneShot { get; set; } = false;
    public bool Regexp { get; set; } = false;
    public bool Repeat { get; set; } = false;
    public bool MultiLine { get; set; } = false;
    public int LinesToMatch { get; set; } = 0;
    public bool WildcardLowerCase { get; set; } = false;
    public string Group { get; set; } = string.Empty;
    public bool IgnoreCase { get; set; } = false;
    public bool KeepEvaluating { get; set; } = false;
    public bool OmitFromLog { get; set; } = false;
    public bool OmitFromOutput { get; set; } = false;
    public bool Inverse { get; set; } = false;
    public bool Italic { get; set; } = false;
    public string Variable { get; set; } = string.Empty;
    private bool byuser = false;
}