namespace Hellclient.Script.Types.Userinput;

public class Datagrid
{
    public string Title { get; set; } = "";
    public string Intro { get; set; } = "";
    public List<Item> Items { get; set; } = new List<Item>();
    public int MaxPage { get; set; }
    public int Page { get; set; }
    public string Filter { get; set; } = "";
    public string OnPage { get; set; } = "";
    public string OnFilter { get; set; } = "";
    public string OnDelete { get; set; } = "";
    public string OnUpdate { get; set; } = "";
    public string OnView { get; set; } = "";
    public string OnCreate { get; set; } = "";
    public string OnSelect { get; set; } = "";
    public void SetPage(int page)
    {
        Page = page;
    }
    public int GetPage()
    {
        return Page;
    }
    public void SetMaxPage(int maxpage)
    {
        MaxPage = maxpage;
    }
    public void SetFilter(string filter)
    {
        Filter = filter;
    }
    public string GetFilter()
    {
        return Filter;
    }
    public void SetOnPage(string onpage)
    {
        OnPage = onpage;
    }
    public void SetOnFilter(string onfilter)
    {
        OnFilter = onfilter;
    }
    public void SetOnDelete(string ondelete)
    {
        OnDelete = ondelete;
    }
    public void SetOnUpdate(string onupdate)
    {
        OnUpdate = onupdate;
    }
    public void SetOnView(string onview)
    {
        OnView = onview;
    }
    public void SetOnCreate(string oncreate)
    {
        OnCreate = oncreate;
    }
    public void SetOnSelect(string onselect)
    {
        OnSelect = onselect;
    }
    public void ResetItems()
    {
        Items = new List<Item>();
    }
    public void Append(string key, string value)
    {
        Items.Add(new Item { Key = key, Value = value });
    }
    public static Datagrid Create(string title, string intro)
    {
        return new Datagrid
        {
            Title = title,
            Intro = intro,
            Page = 1,
        };
    }
}