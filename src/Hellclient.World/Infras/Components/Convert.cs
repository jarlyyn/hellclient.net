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
    public Debounce? Debounce { get; set; }

    public byte[] GetBuffer();
    public void Prompt();
    public void Publish();
    public void AppendBuffer(byte data);
}
public class Convert : IConvert
{
    public void Prompt()
    {
        var line = AnsiHelpers.Parse(CharsetUtil.ToUtf8(Charset, _buffer.ToArray()));
        if (line is null)
        {
            return;
        }
        line.Type = Line.LineTypePrompt;
        OnPrompt?.Invoke(this, line);
    }
    public string Charset { get; set; } = CharsetUtil.UTF8;
    public Debounce? Debounce { get; set; }

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
    public void Publish()
    {
        var line = AnsiHelpers.Parse(CharsetUtil.ToUtf8(Charset, _buffer.ToArray()));
        if (line is null)
        {
            return;
        }
        line.Type = Line.LineTypeReal;
        OnLine?.Invoke(this, line);
        Debounce?.Reset();
        _buffer.Clear();
        var pl = Line.New();
        pl.Type = Line.LineTypePrompt;
        OnPrompt?.Invoke(this, pl);
    }
}