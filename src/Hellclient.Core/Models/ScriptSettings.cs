namespace Hellclient.Core.Models;

public class ScriptSettings
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Intro { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public string OnOpen { get; set; } = string.Empty;
    public string OnClose { get; set; } = string.Empty;
    public string OnConnect { get; set; } = string.Empty;
    public string OnDisconnect { get; set; } = string.Empty;
    public string OnBroadcast { get; set; } = string.Empty;
    public string OnResponse { get; set; } = string.Empty;
    public string OnHUDClick { get; set; } = string.Empty;
    public string OnAssist { get; set; } = string.Empty;
    public string OnBuffer { get; set; } = string.Empty;
    public string OnKeyUp { get; set; } = string.Empty;
    public int OnBufferMin { get; set; } = 0;
    public int OnBufferMax { get; set; } = 0;
    public string OnSubneg { get; set; } = string.Empty;
    public string OnFocus { get; set; } = string.Empty;
    public string OnLoseFocus { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;

}