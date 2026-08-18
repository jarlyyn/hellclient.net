namespace Hellclient.Script.Types.Userinput;

public class DataList
{
    public string Title { get; set; } = "";
    public string Intro { get; set; } = "";
    public List<Item> Items { get; set; } = new List<Item>();
    public bool Mutli { get; set; }
    //Multi的typo,保留兼容性
    
    public List<string> Values { get; set; } = new List<string>();
    public bool WithFilter { get; set; }

    public void SetValues(List<string> values)
    {
        Values = values;
    }
    public void SetMulti(bool multi)
    {
        Mutli = multi;
    }
    public void Append(string key,string value )
    {
        Items.Add(new Item { Key = key, Value = value });
    }
    public static DataList Create(string title, string intro, bool withfilter)
    {
        return new DataList
        {
            Title = title,
            Intro = intro,
            WithFilter = withfilter
        };
    }
}