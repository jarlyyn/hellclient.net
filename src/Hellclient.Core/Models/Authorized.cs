namespace Hellclient.Core.Models;

public class Authorized
{
    public List<string> Permissions { get; set; } = [];
    public List<string> Domains { get; set; } = [];
}