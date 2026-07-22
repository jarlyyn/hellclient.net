namespace Hellclient.World.Types;

public class Mod
{
    public bool Enabled { get; set; } = false;
    public bool Exists { get; set; } = false;
    public List<string> FileList { get; set; } = [];
    public List<string> FolderList { get; set; } = [];

}