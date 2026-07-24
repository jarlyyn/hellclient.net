using Hellclient.World.Infras.Adapters;

namespace Hellclient.World.Types;

public class Word
{
    public string Text { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;

    public string Background { get; set; } = string.Empty;
    public bool Bold { get; set; } = false;
    public bool Underlined { get; set; } = false;
    public bool Blinking { get; set; } = false;
    public bool Inverse { get; set; } = false;
}

public class Line
{
    //通过Print打印
    public const int LineTypePrint = 0;

    //系统信息
    public const int LineTypeSystem = 1;

    //收到的真实信息
    public const int LineTypeReal = 2;

    //输入回显
    public const int LineTypeEcho = 3;

    //输入行类型
    public const int LineTypePrompt = 4;

    //发出的本地广播
    public const int LineTypeLocalBroadcastOut = 5;

    //发出的全局广播
    public const int LineTypeGlobalBroadcastOut = 6;

    //收到的本地广播
    public const int LineTypeLocalBroadcastIn = 7;

    //收到的全局广播
    public const int LineTypeGlobalBroadcastIn = 8;

    //Websocket发出的请求的信息
    public const int LineTypeRequest = 9;

    //Websocket收到的响应的信息
    public const int LineTypeResponse = 10;

    //收到mud发来的非文本信息
    public const int LineTypeSubneg = 11;

    public List<Word> Words { get; set; } = [];
    public string ID { get; set; } = string.Empty;
    public Int64 Timestamp { get; set; } = 0;
    public int Type { get; set; } = 0;
    public bool OmitFromLog { get; set; } = false;
    public bool OmitFromOutput { get; set; } = false;
    public List<string> Triggers { get; set; } = [];
    public string CreatorType { get; set; } = string.Empty;
    public string Creator { get; set; } = string.Empty;

    public string ToPlainText()
    {
        return string.Join("", Words.Select(w => w.Text));
    }
    public void RemoveTail(int length)
    {
        while (length > 0 && Words.Count > 0)
        {
            var lastWord = Words.Last();
            if (lastWord.Text.Length <= length)
            {
                length -= lastWord.Text.Length;
                Words.RemoveAt(Words.Count - 1);
            }
            else
            {
                lastWord.Text = lastWord.Text.Substring(0, lastWord.Text.Length - length);
                length = 0;
            }
        }
    }
    public static Line New()
    {
        return new Line
        {
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Words = [],
            ID = SimpleID.Instance.GenerateID(),
        };
    }
}