using Hellclient.World.Types;

namespace Hellclient.World.Helpers;

public class ScriptDataHelper
{
    public static ScriptInfo ConvertInfo(string id, ScriptData? d)
    {
        var info = new ScriptInfo()
        {
            ID = id,
        };
        if (d != null)
        {
            info.Type = d.Type;
            info.Intro = d.Intro;
            info.Desc = d.Desc;
            info.OnOpen = d.OnOpen;
            info.OnClose = d.OnClose;
            info.OnConnect = d.OnConnect;
            info.OnDisconnect = d.OnDisconnect;
            info.OnAssist = d.OnAssist;
            info.OnKeyUp = d.OnKeyUp;
            info.OnBroadCast = d.OnResponse;
            info.OnResponse = d.OnResponse;
            info.OnBuffer = d.OnBuffer;
            info.OnBufferMin = d.OnBufferMin;
            info.OnBufferMax = d.OnBufferMax;
            info.OnSubneg = d.OnSubneg;
            info.OnHUDClick = d.OnHUDClick;
            info.OnFocus = d.OnFocus;
            info.OnLoseFocus = d.OnLoseFocus;
        }
        return info;
    }
    public static ScriptSettings ConvertSettings(string name,ScriptData? d)
    {
        var settings = new ScriptSettings()
        {
        };
        if (d is not null)
        {
            settings.Name = name;
            settings.Type = d.Type;
            settings.Intro = d.Intro;
            settings.Desc = d.Desc;
            settings.OnOpen = d.OnOpen;
            settings.OnClose = d.OnClose;
            settings.OnConnect = d.OnConnect;
            settings.OnDisconnect = d.OnDisconnect;
            settings.OnBroadcast = d.OnBroadcast;
            settings.OnResponse = d.OnResponse;
            settings.OnAssist = d.OnAssist;
            settings.OnKeyUp = d.OnKeyUp;
            settings.Channel = d.Channel;
            settings.OnHUDClick = d.OnHUDClick;
            settings.OnBuffer = d.OnBuffer;
            settings.OnBufferMin = d.OnBufferMin;
            settings.OnBufferMax = d.OnBufferMax;
            settings.OnFocus = d.OnFocus;
            settings.OnLoseFocus = d.OnLoseFocus;
            settings.OnSubneg = d.OnSubneg;
        }
        return settings;
    }
}