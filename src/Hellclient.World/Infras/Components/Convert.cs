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

    public event EventHandler<TelnetCommand>? OnCommand;
    public void OnByte(object sender, byte data);
    public byte[] GetBuffer();
    public void OnCommandReceived(object sender, TelnetCommand cmd);
    public void Prompt();
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
        OnPrompt?.Invoke(this, line);
    }
    public string Charset { get; set; } = CharsetUtil.UTF8;
    public Debounce? Debounce { get; set; }

    private List<byte> _buffer = new List<byte>();
    public event EventHandler<Line>? OnLine;
    public event EventHandler<Line>? OnPrompt;
    public event EventHandler<TelnetCommand>? OnCommand;
    public byte[] GetBuffer()
    {
        return _buffer.ToArray();
    }
    public void OnByte(object sender, byte data)
    {
        if (data == 13)
        {
            return;
        }
        if (data == 10)
        {
            Publish();
            return;
        }
        _buffer.Add(data);
         Task.Run(async () => await Debounce?.Exec());
    }
    public void OnCommandReceived(object sender, TelnetCommand cmd)
    {
        if (_buffer.Count > 0)
        {
            Publish();
        }
        switch (cmd.Command)
        {
            case TelnetCommand.CmdGoAhead:
                break;
        }
        OnCommand?.Invoke(this, cmd);
    }
    private void Publish()
    {
        var line = AnsiHelpers.Parse(CharsetUtil.ToUtf8(Charset, _buffer.ToArray()));
        if (line is null)
        {
            return;
        }
        OnLine?.Invoke(this, line);
        Debounce?.Reset();
        _buffer.Clear();
    }
}