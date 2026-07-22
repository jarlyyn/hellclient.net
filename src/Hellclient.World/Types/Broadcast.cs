namespace Hellclient.World.Types;

public class Broadcast
{
	public string ID { get; set; } = string.Empty;
	public string Channel { get; set; } = string.Empty;
	public string Message { get; set; } = string.Empty;
	public bool Global { get; set; } = false;

}