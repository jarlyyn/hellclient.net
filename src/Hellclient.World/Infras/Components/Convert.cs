using System.Text;
using Hellclient.World.Helpers;
using Hellclient.World.Types;
using Hellclient.World.Utils;

namespace Hellclient.World.Infras.Components;

public interface IConvert
{
    public string Charset { get; set; }
    public event EventHandler<Line>? OnLine;
    public event EventHandler<Line>? OnPrompt;
    public byte[] GetBuffer();
    public void SendPrompt();
    public void Publish();
    public void PublishPrompt();
    public void AppendBuffer(byte data);
}
public class Convert : IConvert
{
    public void SendPrompt()
    {
        OnPrompt?.Invoke(this, PromptLine?? EmptyLine);
    }
    public string Charset { get; set; } = CharsetUtil.UTF8;
    private readonly Line EmptyLine = Line.NewWithType(Line.LineTypeReal);
    public Line? PromptLine { get; set; } = null;
    public List<byte> _buffer = new List<byte>();
    public event EventHandler<Line>? OnLine;
    public event EventHandler<Line>? OnPrompt;
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
        var line = AnsiHelpers.Parse(CharsetUtil.ToUtf8(Charset, _buffer.ToArray()));
        if (line is null)
        {
            return;
        }
        _buffer.Clear();
        PromptLine = line;
        line.Type = Line.LineTypePrompt;
        SendPrompt();
    }
    public void Publish()
    {
        var line = AnsiHelpers.Parse(CharsetUtil.ToUtf8(Charset, _buffer.ToArray()));
        _buffer.Clear();
        if (line is null)
        {
            return;
        }
        PromptLine = null;
        line.Type = Line.LineTypeReal;
        OnLine?.Invoke(this, line);
    }
}