using System.Text;
using Hellclient.World.Helpers;
using Hellclient.World.Types;
using Hellclient.World.Utils;

namespace Hellclient.World.Infras.Components;

public interface IConvert
{
    public string Charset { get; set; }
    public event EventHandler<Line>? OnLine;
    public byte[] GetBuffer();
    //废弃
    public void SendPrompt();
    public void Publish();
    //废弃
    public void PublishPrompt();
    public void AppendBuffer(byte data);
    public void AddAnsi(string data);
    public string LastAnsi{get;}
    public void Reset();
}
public class Convert : IConvert
{

    public void SendPrompt()
    {
        // OnPrompt?.Invoke(this, PromptLine ?? EmptyLine);
    }
    public string Charset { get; set; } = CharsetUtil.UTF8;
    public List<byte> _buffer = new List<byte>();
    public event EventHandler<Line>? OnLine;
    private List<AnsiLine> PendingLines { get; set; } = new();
    public string LastAnsi { get; set; } = "";
    public void AddAnsi(string data)
    {
        var line = AnsiHelpers.Parse(data);
        if (line is not null)
        {
            line.Type = Line.LineTypeReal;
            PendingLines.Add(new AnsiLine(line, data));
            if (PendingLines.Count == 1)
            {
                ExecLines();
            }
        }
    }
    private void ExecLines()
    {
        while (PendingLines.Count > 0)
        {
            var current = PendingLines[0];
            PendingLines.RemoveAt(0);
            LastAnsi = current.Ansi;
            OnLine?.Invoke(this, current.Line);
        }
    }
    public byte[] GetBuffer()
    {
        return _buffer.ToArray();
    }
    public void AppendBuffer(byte data)
    {
        _buffer.Add(data);
    }
    public void PublishPrompt()
    {
    }
    public void Reset()
    {
        _buffer.Clear();
    }
    public void Publish()
    {
        var data = CharsetUtil.ToUtf8(Charset, _buffer.ToArray());
        _buffer.Clear();
        AddAnsi(data);
    }
}