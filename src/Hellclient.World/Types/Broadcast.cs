using Hellclient.World.Infras.Adapters;

namespace Hellclient.World.Types;

public class Broadcast
{
	public string ID { get; set; } = string.Empty;
	public string Channel { get; set; } = string.Empty;
	public string Message { get; set; } = string.Empty;
	public bool Global { get; set; } = false;
	public static Broadcast CreateBroadcast(string channel, string message, bool global)
	{
		return new Broadcast
		{
			ID = SimpleID.Instance.GenerateID(),
			Channel = channel,
			Message = message,
			Global = global
		};
	}
}