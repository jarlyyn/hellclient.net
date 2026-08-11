using System.Data.Common;
using Hellclient.World.Types;
using Hellclient.World.Utils;

namespace Hellclient.World.Components.Automation;

public class Triggers
{
    public Dictionary<string, AutomationTrigger> All { get; set; } = new();
    public Dictionary<string, AutomationTrigger> ByUser { get; set; } = new();
    public Dictionary<string, AutomationTrigger> ByScript { get; set; } = new();
    public Dictionary<string, AutomationTrigger> Named { get; set; } = new();
    public Dictionary<string, AutomationTrigger> Temporary { get; set; } = new();
    public Dictionary<string, Dictionary<string, AutomationTrigger>> Grouped { get; set; } = new();
    //是否需要重排
    bool Disorder { get; set; } = true;
    //缓存的有顺序的触发器队列
    private List<AutomationTrigger> cachedQueue = new();
    private AutomationTrigger? unloadTrigger(string id)
    {
        var tri = All.TryGetValue(id, out var t) ? t : null;
        if (tri is null)
        {
            return null;
        }
        All.Remove(id);
        if (tri.Data.ByUser())
        {
            ByUser.Remove(id);
        }
        else
        {
            ByScript.Remove(id);
        }
        if (tri.Data.Name != "")
        {
            Named.Remove(tri.Data.Name);
        }
        if (tri.Data.Group != "")
        {
            var group = Grouped.TryGetValue(tri.Data.Group, out var g) ? g : null;
            if (group is not null)
            {
                group.Remove(id);
                if (group.Count == 0)
                {
                    Grouped.Remove(tri.Data.Group);
                }
            }
        }
        if (tri.Data.Temporary)
        {
            Temporary.Remove(id);
        }
        Disorder = true;
        tri.Matcher = null;
        return tri;
    }
    private void loadTrigger(AutomationTrigger trigger)
    {
        var trdata = trigger.Data;
        All[trdata.ID] = trigger;
        if (!trdata.Temporary)
        {
            if (trdata.ByUser())
            {
                ByUser[trdata.ID] = trigger;
            }
            else
            {
                ByScript[trdata.ID] = trigger;
            }
        }
        if (trdata.Name != "")
        {
            Named[trdata.PrefixedName()] = trigger;
        }
        if (trdata.Group != "")
        {
            if (!Grouped.ContainsKey(trdata.Group))
            {
                Grouped[trdata.Group] = new();
            }
            Grouped[trdata.Group][trdata.ID] = trigger;
        }
        if (trdata.Temporary)
        {
            Temporary[trdata.ID] = trigger;
        }
        Disorder = true;
    }
    private AutomationTrigger createTrigger(Trigger data)
    {
        var trigger = new AutomationTrigger
        {
            Data = data
        };
        return trigger;
    }
    private bool addTrigger(Trigger data)
    {
        if (All.ContainsKey(data.ID))
        {
            return false;
        }
        var trigger = createTrigger(data);
        loadTrigger(trigger);
        return true;
    }
    private bool removeTrigger(string id)
    {
        var t = unloadTrigger(id);
        if (t is null)
        {
            return false;
        }
        //处理被删除，但还在队列的情况
        t.Deleted = true;
        return true;
    }
    public List<AutomationTrigger> Queue()
    {
        if (!Disorder)
        {
            return cachedQueue;
        }
        var queue = new List<AutomationTrigger>();
        All.Values.ToList().ForEach(t =>
        {
            if (!t.Deleted && t.Data.Enabled)
            {
                queue.Add(t);
            }
        });
        queue.Sort((a, b) => a.Data.Sequence.CompareTo(b.Data.Sequence));
        cachedQueue = queue;
        Disorder = false;
        return cachedQueue;
    }
    public bool AddTrigger(Trigger tr, bool replace)
    {
        if (tr.Name != "")
        {
            var named = Named.TryGetValue(tr.PrefixedName(), out var t) ? t : null;
            if (named is not null)
            {
                if (!replace)
                {
                    return false;
                }
                removeTrigger(named.Data.ID);
            }
        }
        return addTrigger(tr);
    }
    public bool RemoveTrigger(string id)
    {
        return removeTrigger(id);
    }
    public void AddTriggers(List<Trigger> triggers)
    {
        triggers.ForEach(t =>
        {
            addTrigger(t);
        });
    }
    public void DoDeleteTriggerByType(bool byuser)
    {
        var list = byuser ? ByUser.Values.ToList() : ByScript.Values.ToList();
        foreach (var t in list)
        {
            removeTrigger(t.Data.ID);
        }
    }
    public Trigger? GetTrigger(string id)
    {
        var tr = All.TryGetValue(id, out var t) ? t : null;
        if (tr is null)
        {
            return null;
        }
        return tr.Data;
    }
    public int DoUpdateTrigger(Trigger ti)
    {
        var old = All.TryGetValue(ti.ID, out var existing) ? existing : null;
        if (old is null)
        {
            return MushString.UpdateFailNotFound;
        }
        ti.SetByUser(old.Data.ByUser());
        if (ti.Name != "" && ti.Name != old.Data.Name && Named.ContainsKey(ti.PrefixedName()))
        {
            return MushString.UpdateFailDuplicateName;
        }
        unloadTrigger(old.Data.ID);
        old.Data = ti;
        loadTrigger(old);
        return MushString.UpdateOK;
    }
    public bool DoDeleteTriggerByName(string name)
    {
        var tr = Named.TryGetValue(name, out var t) ? t : null;
        if (tr is null)
        {
            return false;
        }
        removeTrigger(tr.Data.ID);
        return true;
    }
    public int DoDeleteTemporaryTriggers()
    {
        var list = Temporary.Values.ToList();
        foreach (var t in list)
        {
            removeTrigger(t.Data.ID);
        }
        return list.Count;
    }
    public int DoDeleteTriggerGroup(string group, bool byUser)
    {
        int count = 0;
        var groupTriggers = Grouped.TryGetValue(group, out var g) ? g : null;
        if (groupTriggers is null)
        {
            return count;
        }
        var list = groupTriggers.Values.ToList();
        foreach (var t in list)
        {
            if (t.Data.ByUser() == byUser)
            {
                removeTrigger(t.Data.ID);
                count++;
            }
        }
        return count;
    }
    public bool DoEnableTriggerByName(string name, bool enable)
    {
        var tr = Named.TryGetValue(name, out var t) ? t : null;
        if (tr is null)
        {
            return false;
        }
        tr.Data.Enabled = enable;
        Disorder = true;
        return true;
    }
    public int DoEnableTriggerGroup(string group, bool enable)
    {
        int count = 0;
        var groupTriggers = Grouped.TryGetValue(group, out var g) ? g : null;
        if (groupTriggers is null)
        {
            return count;
        }
        var list = groupTriggers.Values.ToList();
        count = list.Count;
        foreach (var t in list)
        {
            t.Data.Enabled = enable;
        }
        Disorder = true;
        return count;
    }
    public List<Trigger> GetTriggersByType(bool byUser)
    {
        return byUser ? ByUser.Values.Select(t => t.Data).ToList() : ByScript.Values.Select(t => t.Data).ToList();
    }
    public FoundStringResult GetTriggerOption(string name, string option)
    {
        var tr = Named.TryGetValue(name, out var t) ? t : null;
        if (tr is null)
        {
            return FoundStringResult.NotFound;
        }
        return tr.Option(option).Found();
    }
    public FoundStringResult GetTriggerInfo(string name, int info)
    {
        var tr = Named.TryGetValue(name, out var t) ? t : null;
        if (tr is null)
        {
            return FoundStringResult.NotFound;
        }
        return tr.Info(info).Found();
    }
    public FoundBoolResult SetTriggerOption(string name, string option, string value)
    {
        var tr = Named.TryGetValue(name, out var t) ? t : null;
        if (tr is null)
        {
            return FoundBoolResult.NotFound;
        }
        return tr.SetOption(option, value).Found();
    }
    public bool HasNamedTrigger(string name)
    {
        return Named.ContainsKey(name);
    }
    public List<string> DoListTriggerNames(bool byuser)
    {
        var list = byuser ? ByUser.Values.ToList() : ByScript.Values.ToList();
        return list.Select(t => t.Data.Name).ToList();
    }
}