using Hellclient.World.States;
using Hellclient.World.Types;

namespace Hellclient.World.Features.Services;

public interface IInfoService
{
    public void InstallTo(WorldContext context);
    public List<Line> CurrentLines(WorldContext context);
    public Line? CurrentPrompt(WorldContext context);
    public ClientInfo ClientInfo(WorldContext context);
    public void AddHistory(WorldContext context, string line);
    public List<string> GetHistories(WorldContext context);
    public void FlushHistories(WorldContext context);
    public void OmitOutput(WorldContext context);
    public void DeleteLines(WorldContext context, int count);
    public int GetLineCount(WorldContext context);
    public List<Line> GetRecentLines(WorldContext context, int count);
    public int GetLinesInBufferCount(WorldContext context);
    public Line? GetLine(WorldContext context, int idx);
    public void SetPriority(WorldContext context, int priority);
    public int GetPriority(WorldContext context);
    public void SetSummary(WorldContext context, List<Line> lines);
    public List<Line> GetSummary(WorldContext context);
    public long GetLastActive(WorldContext context);
    void UpdateLastActive(WorldContext context);
}

public class InfoService : IInfoService
{
    public void InstallTo(WorldContext context)
    {
        context.EventBus.LineEvent += (sender, line) =>
        {
            onNewLine(context, line);
        };
        context.EventBus.PromptEvent += (sender, line) =>
        {
            onPrompt(context, line);
        };

    }
    private void onNewLine(WorldContext context, Line line)
    {
        context.Info.Lines.Add(line);
        if (line.Type == Line.LineTypeReal)
        {
            context.Info.Recent.Add(line);
        }
        linesUpdated(context);
    }
    private void onPrompt(WorldContext context, Line line)
    {
        context.Info.Prompt = line;
    }

    public void OmitOutput(WorldContext context)
    {
        var line = context.Info.Lines.GetCurrentItem();
        if (line != null)
        {
            line.OmitFromOutput = true;
            linesUpdated(context);
        }
    }
    public void DeleteLines(WorldContext context, int count)
    {
        context.Info.Lines.DeleteItems(count);
        linesUpdated(context);
    }
    public List<Line> GetRecentLines(WorldContext context, int count)
    {
        return context.Info.Lines.GetRecentItems(count);
    }
    public ClientInfo ClientInfo(WorldContext context)
    {
        return new ClientInfo()
        {
            ID = context.ID,
            HostPort = $"{context.Connection.Host}:{context.Connection.Port}",
            // ReadyAt=context.Info.ReadyAt,
            Running = context.Connection.IsConnected(),
            // ScriptID=
            Priority = context.Info.Priority,
            Summary = context.Info.Summary,
            LastActive = context.Info.LastActive,
            // Position=context.Info.Lines.Position,
        };
    }
    private void linesUpdated(WorldContext context)
    {
        var updatedLines = lines(context);
        context.EventBus.LinesEvent?.Invoke(this, updatedLines);
    }
    private List<Line> lines(WorldContext context)
    {
        var result = context.Info.Lines.GetAllItems();
        return result.FindAll(x => x != null && !x.OmitFromOutput);
    }
    public List<Line> CurrentLines(WorldContext context)
    {
        return lines(context);
    }
    public Line? CurrentPrompt(WorldContext context)
    {
        return context.Info.Lines.GetCurrentItem();
    }
    public int GetLineCount(WorldContext context)
    {
        return context.Info.Lines.Count();
    }
    public void AddHistory(WorldContext context, string line)
    {
        context.Info.History.Add(line);
    }
    public List<string> GetHistories(WorldContext context)
    {
        return context.Info.History.GetAllItems();
    }
    public void FlushHistories(WorldContext context)
    {
        context.Info.History.Flush();
    }
    public void CurrentHistories(WorldContext context)
    {
        var histories = context.Info.History.GetAllItems();
        context.EventBus.HistoriesEvent?.Invoke(this, histories);
    }
    public int GetLinesInBufferCount(WorldContext context)
    {
        return context.Info.Lines.Count();
    }
    public Line? GetLine(WorldContext context, int idx)
    {
        if (idx < 0 || idx >= context.Info.Lines.Count())
        {
            return null;
        }
        return context.Info.Lines.GetItemByIndex(idx);
    }
    public void SetPriority(WorldContext context, int priority)
    {
        context.Info.Priority = priority;
    }
    public int GetPriority(WorldContext context)
    {
        return context.Info.Priority;
    }
    public void SetSummary(WorldContext context, List<Line> lines)
    {
        context.Info.Summary = lines;
    }
    public List<Line> GetSummary(WorldContext context)
    {
        return context.Info.Summary;
    }
    public long GetLastActive(WorldContext context)
    {
        return context.Info.LastActive;
    }
    public void UpdateLastActive(WorldContext context)
    {
        context.Info.LastActive = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
    public void RefreshClientInfo(WorldContext context)
    {
        var info = ClientInfo(context);
        context.EventBus.ClientInfoEvent?.Invoke(this, info);
    }
}