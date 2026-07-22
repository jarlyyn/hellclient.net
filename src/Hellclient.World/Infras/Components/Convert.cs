using Hellclient.World.Helpers;
using Hellclient.World.Types;
using Hellclient.World.Utils;

namespace Hellclient.World.Infras.Components;

public interface IConvert
{
    public string Charset { get; set; }
    public event EventHandler<Line>? OnLine;
    public event EventHandler<TelnetCommand>? OnCommand;
    public void OnByte(object sender, byte data);
    public void OnCommandReceived(object sender, TelnetCommand cmd);
}
public class Convert : IConvert
{
    public string Charset { get; set; } = CharsetUtil.UTF8;
    private List<byte> _buffer = new List<byte>();
    public event EventHandler<Line>? OnLine;
    public event EventHandler<TelnetCommand>? OnCommand;
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
    }
    public void OnCommandReceived(object sender, TelnetCommand cmd)
    {
        if (_buffer.Count > 0)
        {
            Publish();
        }
        OnCommand?.Invoke(this, cmd);
    }
    private void Publish()
    {

        OnLine?.Invoke(this, AnsiHelpers.Parse(CharsetUtil.ToUtf8(Charset, _buffer.ToArray())));
        _buffer.Clear();
    }
}