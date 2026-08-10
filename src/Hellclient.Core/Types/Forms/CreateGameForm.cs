namespace Hellclient.Core.Types.Forms;

public class CreateGameForm
{
	public string ID { get; set; } = "";
	public string Host { get; set; } = "";
	public string Port { get; set; } = "";
	public string Charset { get; set; } = "";
}