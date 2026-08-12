namespace Hellclient.Core.Types.Forms;

public class UpdatePasswordForm
{
	public string Username { get; set; } = "";
	public string Password { get; set; } = "";
	public string RepeatPassword { get; set; } = "";
}