using Hellclient.World.Types;

namespace Hellclient.World.Cores;

public partial class World
{
    public int GetHUDSize() => Service.HudService.GetSize(Context);
    public void SetHUDSize(int size) => Service.HudService.SetSize(Context, size);
    public List<Line> GetHUDContent() => Service.HudService.GetContent(Context);
    public bool UpdateHUDContent(int start, List<Line> content) => Service.HudService.UpdateContent(Context, start, content);

}