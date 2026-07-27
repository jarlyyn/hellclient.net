using Hellclient.World.States;
using Hellclient.World.Types;

namespace Hellclient.World.Features.Services;

public interface IHUDService
{
    public void SetSize(WorldContext context, int size);
    public int GetSize(WorldContext context);
    public bool UpdateContent(WorldContext context, int start, List<Line> lines);
    public List<Line> GetContent(WorldContext context);
}
public class HUDService : IHUDService
{
    public void SetSize(WorldContext context, int size)
    {
        if (size < 0)
        {
            size = 0;
        }
        if (size > HUD.MaxSize)
        {
            size = HUD.MaxSize;
        }
        context.HUD = new HUD();
        for (int i = 0; i < size; i++)
        {
            context.HUD.Content.Add(new Line() { });
        }
        context.EventBus.HUDContentEvent?.Invoke(this, context.HUD.Content);
    }
    public int GetSize(WorldContext context)
    {
        return context.HUD.Content.Count;
    }
    public bool UpdateContent(WorldContext context, int start, List<Line> lines)
    {
        if (start < 0 || start + lines.Count - 1 > context.HUD.Content.Count)
        {
            return false;
        }
        for (int i = 0; i < lines.Count; i++)
        {
            context.HUD.Content[start + i] = lines[i];
        }
        context.EventBus.HUDUpdateEvent?.Invoke(this, new DiffLines(start, lines));
        return true;
    }
    public List<Line> GetContent(WorldContext context)
    {
        return context.HUD.Content;
    }
}