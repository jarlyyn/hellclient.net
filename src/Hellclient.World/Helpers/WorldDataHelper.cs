using Hellclient.World.Types;

namespace Hellclient.World.Helpers;

public class WorldDataHelper
{
    public static WorldSettings ConvertSettings(string id,WorldData? d)
    {
        var settings = new WorldSettings()
        {
            ID = id,
        };
        if (d is not null)
        {
		settings.Host = d.Host;
		settings.Port = d.Port;
		settings.Proxy = d.Proxy;
		settings.Charset = d.Charset;
		settings.Name = d.Name;
		settings.CommandStackCharacter = d.CommandStackCharacter;
		settings.ScriptPrefix = d.ScriptPrefix;
		settings.ShowBroadcast = d.ShowBroadcast;
		settings.ShowSubneg = d.ShowSubneg;
		settings.ModEnabled = d.ModEnabled;
		settings.AutoSave = d.AutoSave;
		settings.IgnoreBatchCommand = d.IgnoreBatchCommand;
        }
        return settings;
    }
}
