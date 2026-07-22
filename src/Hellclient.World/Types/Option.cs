namespace Hellclient.World.Types;

public class Option
{
    public List<string> Blacklist { get; set; } = [];
    public List<string> Whitelist { get; set; } = [];
    public List<List<string>> BlockedPath { get; set; } = [];
  
}