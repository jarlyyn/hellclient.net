namespace Hellclient.World.Types;
public class SystemConfig
{
    public string Addr { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Switch { get; set; } = string.Empty;
    public string DefaultServer { get; set; } = string.Empty;
    public string DefaultCharset { get; set; } = string.Empty;
    public string TerminalType { get; set; } = string.Empty;
    public string URL { get; set; } = string.Empty;
    public int MaxHistory { get; set; } = 0;
    public int MaxLines { get; set; } = 0;
    public int MaxRecent { get; set; } = 0;
    public int LinesPerScreen { get; set; } = 0;
    public long ConnectTimeout { get; set; } = 0;
 
}