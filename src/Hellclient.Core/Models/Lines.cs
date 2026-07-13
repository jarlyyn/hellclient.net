namespace Hellclient.Core.Models;

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
    const int LineTypePrint = 0;

    //系统信息
    const int LineTypeSystem = 1;

    //收到的真实信息
    const int LineTypeReal = 2;

    //输入回显
    const int LineTypeEcho = 3;

    //输入行类型
    const int LineTypePrompt = 4;

    //发出的本地广播
    const int LineTypeLocalBroadcastOut = 5;

    //发出的全局广播
    const int LineTypeGlobalBroadcastOut = 6;

    //收到的本地广播
    const int LineTypeLocalBroadcastIn = 7;

    //收到的全局广播
    const int LineTypeGlobalBroadcastIn = 8;

    //Websocket发出的请求的信息
    const int LineTypeRequest = 9;

    //Websocket收到的响应的信息
    const int LineTypeResponse = 10;

    //收到mud发来的非文本信息
    const int LineTypeSubneg = 11;

    public List<Word> Words { get; set; } = [];
    public string ID { get; set; } = string.Empty;
    public Int64 Timestamp { get; set; } = 0;
    public int Type { get; set; } = 0;
    public bool OmitFromLog { get; set; } = false;
    public bool OmitFromOutput { get; set; } = false;
    public List<string> Triggers { get; set; } = [];
    public string CreatorType { get; set; } = string.Empty;
    public string Creator { get; set; } = string.Empty;
}