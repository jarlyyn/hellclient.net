namespace Hellclient.Core.Contexts;
using Hellclient.Core.Models;
public partial class MainContext
{
    public static MainContext Instance { get; set; } = new MainContext();
}