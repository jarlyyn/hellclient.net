namespace Hellclient.Script.Types.Userinput;

public class VisualPrompt
{
    const string MediaTypeImage = "image";
    const string MediaTypeSlide = "base64slide";

    public string Title { get; set; } = "";
    public string Intro { get; set; } = "";
    public string Source { get; set; } = "";
    public string MediaType { get; set; } = "";
    public string Value { get; set; } = "";
    public List<Item> Items { get; set; } = new List<Item>();
    public bool Portrait { get; set; }
    public string RefreshCallback { get; set; } = "";

    public bool IsURL()
    {
        var t = MediaType.ToLower();
        return t == MediaTypeImage || t == MediaTypeSlide;
    }
    public void SetMediaType(string mediaType)
    {
        MediaType = mediaType;
    }
    public void SetPortrait(bool portrait)
    {
        Portrait = portrait;
    }
    public void SetRefreshCallback(string refreshCallback)
    {
        RefreshCallback = refreshCallback;
    }
    public void Append(string key, string value)
    {
        Items.Add(new Item { Key = key, Value = value });
    }
    public void SetValue(string value)
    {
        Value = value;
    }
    public static VisualPrompt Create(string title, string intro, string source)
    {
        return new VisualPrompt
        {
            Title = title,
            Intro = intro,
            Source = source,
            Portrait = false,
            MediaType = MediaTypeImage
        };
    }
}