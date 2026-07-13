namespace Hellclient.Core.Contexts;
using Hellclient.Core.Models;
using Hellclient.Core.Interfaces;
using Hellclient.Core.Adapters;
public partial class MainContext
{
    public static MainContext Instance { get; set; } = new MainContext();
    public IUniqueid UniqueID { get; set; } = new SimpleID();
}