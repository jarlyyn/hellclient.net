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
        if (d == null)
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
}