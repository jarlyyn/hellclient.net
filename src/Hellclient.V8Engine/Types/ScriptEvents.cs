namespace Hellclient.V8Engine.Types;

public class ScriptEvents
{
    public string OnOpen { get; set; } = "";
	public string OnClose{get;set;}="";
	public string OnConnect{get;set;}="";
	public string OnDisconnect{get;set;}="";
	public string OnBroadcast {get;set;}="";
	public string OnResponse {get;set;}="";
	public string OnHUDClick {get;set;}="";
	public string OnBuffer {get;set;}="";
	public string OnSubneg {get;set;}="";
	public int OnBufferMax {get;set;}=0;
	public int OnBufferMin {get;set;}=0;
	public string OnFocus {get;set;}="";
	public string OnLoseFocus{get;set;}="";
	public string OnKeyUp {get;set;}="";

}