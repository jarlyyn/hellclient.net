using Hellclient.World.Types;
using Hellclient.World.Utils;

namespace Hellclient.World.Components.Automation;

public class Aliases
{
    Dictionary<string, AutomationAlias> All { get; set; } = new();
    Dictionary<string, AutomationAlias> ByUser { get; set; } = new();
    Dictionary<string, AutomationAlias> ByScript { get; set; } = new();
    Dictionary<string, AutomationAlias> Named { get; set; } = new();
    Dictionary<string, AutomationAlias> Temporary { get; set; } = new();
    Dictionary<string, Dictionary<string, AutomationAlias>> Grouped { get; set; } = new();
    bool Disorder { get; set; } = true;
    List<AutomationAlias> cachedQueue = new();
    public AutomationAlias? unloadAlias(string id)
    {
        var al = All.TryGetValue(id, out var t) ? t : null;
        if (al is null)
        {
            return null;
        }
        All.Remove(id);
        if (al.Data.ByUser())
        {
            ByUser.Remove(al.Data.ID);
        }
        else
        {
            ByScript.Remove(al.Data.ID);
        }
        if (al.Data.Name != "")
        {
            Named.Remove(al.Data.PrefixedName());
        }
        if (al.Data.Group != "")
        {
            var group = Grouped.TryGetValue(al.Data.Group, out var g) ? g : null;
            if (group is not null)
            {
                group.Remove(al.Data.ID);
                if (group.Count == 0)
                {
                    Grouped.Remove(al.Data.Group);
                }
            }
        }
        if (al.Data.Temporary)
        {
            Temporary.Remove(al.Data.ID);
        }
        Disorder = true;
        al.Matcher = null;
        return al;
    }
    public void loadAlias(AutomationAlias alias)
    {
        var aldata = alias.Data;
        All[aldata.ID] = alias;
        if (!aldata.Temporary)
        {
            if (aldata.ByUser())
            {
                ByUser[aldata.ID] = alias;
            }
            else
            {
                ByScript[aldata.ID] = alias;
            }
        }
        if (aldata.Name != "")
        {
            Named[aldata.PrefixedName()] = alias;
        }
        if (aldata.Group != "")
        {
            if (!Grouped.ContainsKey(aldata.Group))
            {
                Grouped[aldata.Group] = [];
            }
            Grouped[aldata.Group][aldata.ID] = alias;
        }
        if (aldata.Temporary)
        {
            Temporary[aldata.ID] = alias;
        }
    }
    private AutomationAlias createAlias(Alias al)
    {
        return new AutomationAlias()
        {
            Data = al,
        };
    }
    private bool addAlias(Alias al)
    {
        if (All.ContainsKey(al.ID))
        {
            return false;
        }
        var alias = createAlias(al);
        loadAlias(alias);
        return true;
    }
    private bool removeAlias(string id)
    {
        var al = unloadAlias(id);
        if (al is null)
        {
            return false;
        }
        al.Deleted = true;
        return true;
    }
    public List<AutomationAlias> Queue()
    {
        if (!Disorder)
        {
            return cachedQueue;
        }
        cachedQueue = All.Values.ToList();
        cachedQueue.Sort((a, b) => a.CompareTo(b));
        Disorder = false;
        return cachedQueue;
    }
    public bool RemoveAlias(string id)
    {
        return removeAlias(id);
    }
    public bool AddAlias(Alias al, bool replace)
    {
        if (al.Name != "")
        {
            var name = al.PrefixedName();
            var named = Named.TryGetValue(name, out var t) ? t : null;
            if (named is not null)
            {
                if (!replace)
                {
                    return false;
                }
                removeAlias(named.Data.ID);
            }
        }
        return addAlias(al);
    }
    public void AddAliases(List<Alias> aliases)
    {
        foreach (var al in aliases)
        {
            addAlias(al);
        }
    }
    public void DoDeleteAliasByType(bool byuser)
    {
        var list = (byuser ? ByUser : ByScript).Values.ToList();
        foreach (var t in list)
        {
            removeAlias(t.Data.ID);
        }
    }
    public Alias? GetAlias(string id)
    {
        var result = All.TryGetValue(id, out var alias) ? alias : null;
        return result?.Data;
    }
    public int DoUpdateAlias(Alias al)
    {
        var old = All.TryGetValue(al.ID, out var existing) ? existing : null;
        if (old is null)
        {
            return MushString.UpdateFailNotFound;
        }
        al.SetByUser(old.Data.ByUser());
        if (al.Name != "" && al.Name != old.Data.Name && Named.ContainsKey(al.PrefixedName()))
        {
            return MushString.UpdateFailDuplicateName;
        }
        unloadAlias(old.Data.ID);
        old.Data = al;
        loadAlias(old);
        return MushString.UpdateOK;
    }
    public bool DoDeleteAliasByName(string name)
    {
        var named = Named.TryGetValue(name, out var t) ? t : null;
        if (named is null)
        {
            return false;
        }
        removeAlias(named.Data.ID);
        return true;
    }
    public int DoDeleteTemporaryAliases()
    {
        var list = Temporary.Values.ToList();
        foreach (var t in list)
        {
            removeAlias(t.Data.ID);
        }
        return list.Count;
    }
    public int DoDeleteAliasGroup(string group, bool byUser)
    {
        var groupTriggers = Grouped.TryGetValue(group, out var g) ? g : null;
        if (groupTriggers is null)
        {
            return 0;
        }
        int count = 0;
        var list = groupTriggers.Values.ToList();
        foreach (var t in list)
        {
            if (t.Data.ByUser() == byUser)
            {
                removeAlias(t.Data.ID);
                count++;
            }
        }
        return count;
    }
    public int DoEnableAliasGroup(string group, bool enable)
    {
        var groupTriggers = Grouped.TryGetValue(group, out var g) ? g : null;
        if (groupTriggers is null)
        {
            return 0;
        }
        var list = groupTriggers.Values.ToList();
        foreach (var t in list)
        {
            t.Data.Enabled = enable;
        }
        Disorder = true;
        return list.Count;
    }
    public int DoDeleteAliasByGroup(string group, bool byUser)
    {
        var groupTriggers = Grouped.TryGetValue(group, out var g) ? g : null;
        if (groupTriggers is null)
        {
            return 0;
        }
        int count = 0;
        var list = groupTriggers.Values.ToList();
        foreach (var t in list)
        {
            if (t.Data.ByUser() == byUser)
            {
                removeAlias(t.Data.ID);
                count++;
            }
        }
        return count;
    }
    public bool DoEnableAliasByName(string name, bool enabled)
    {
        var al = Named.TryGetValue(name, out var t) ? t : null;
        if (al is null)
        {
            return false;
        }
        al.Data.Enabled = enabled;
        Disorder = true;
        return true;
    }
    public List<Alias> GetAliasesByType(bool byUser)
    {
        var list = (byUser ? ByUser : ByScript).Values.ToList();
        return list.Select(t => t.Data).ToList();
    }
    public FoundStringResult GetAliasOption(string name, string option)
    {
        var al = Named.TryGetValue(name, out var t) ? t : null;
        if (al is null)
        {
            return FoundStringResult.NotFound;
        }
        return al.Option(option).Found();
    }
    public FoundStringResult GetAliasInfo(string name, int infotype)
    {
        var al = Named.TryGetValue(name, out var t) ? t : null;
        if (al is null)
        {
            return FoundStringResult.NotFound;
        }
        return al.Info(infotype).Found();
    }
    public FoundBoolResult SetAliasOption(string name, string option, string value)
    {
        var al = Named.TryGetValue(name, out var t) ? t : null;
        if (al is null)
        {
            return FoundBoolResult.NotFound;
        }
        return al.SetOption(option, value).Found();
    }
    public bool HasNamedAlias(string name)
    {
        return Named.ContainsKey(name);
    }
    public List<string> DoListAliasNames(bool byUser)
    {
        var list = (byUser ? ByUser : ByScript).Values.ToList();
        return list.Select(t => t.Data.Name).ToList();
    }
}