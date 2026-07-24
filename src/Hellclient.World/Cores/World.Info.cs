

using Hellclient.World.Types;

namespace Hellclient.World.Cores;


public partial class World
{
    public List<Line> GetCurrentLines() => Service.InfoService.CurrentLines(Context);
    public Line? GetPrompt() => Service.InfoService.CurrentPrompt(Context);
    public ClientInfo? GetClientInfo() => Service.InfoService.ClientInfo(Context);
    public void AddHistory(string value) => Service.InfoService.AddHistory(Context, value);
    public List<string> GetHistories() => Service.InfoService.GetHistories(Context);
    public void FlushHistories() => Service.InfoService.FlushHistories(Context);
    public void DoOmitOutput() => Service.InfoService.OmitOutput(Context);
    public void DoDeleteLines(int count) => Service.InfoService.DeleteLines(Context, count);
    public int GetLineCount() => Service.InfoService.GetLineCount(Context);
    public List<Line> GetRecentLines(int count) => Service.InfoService.GetRecentLines(Context, count);
    public int GetLinesInBufferCount() => Service.InfoService.GetLinesInBufferCount(Context);
    public Line? GetLine(int idx) => Service.InfoService.GetLine(Context, idx);
    public void SetPriority(int priority) => Service.InfoService.SetPriority(Context, priority);
    public int GetPriority() => Service.InfoService.GetPriority(Context);
    public void SetSummary(List<Line> lines) => Service.InfoService.SetSummary(Context, lines);
    public List<Line> GetSummary() => Service.InfoService.GetSummary(Context);
    public void UpdateLastActive() => Service.InfoService.UpdateLastActive(Context);
    public long GetLastActive() => Service.InfoService.GetLastActive(Context);

}