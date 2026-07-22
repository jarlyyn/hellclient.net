namespace Hellclient.Core.Features.States;
using Hellclient.Core.Types;
public partial class MainContext
{
    public static MainContext Instance { get; set; } = new MainContext();
}