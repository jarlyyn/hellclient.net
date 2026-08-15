namespace Hellclient.World.Components.Automation;

using Hellclient.World.Types;
using Hellclient.World.Utils;
using Timer = Hellclient.World.Types.Timer;
public class Timers
{
    Dictionary<string, AutomationTimer> All { get; set; } = [];
    Dictionary<string, AutomationTimer> ByUser { get; set; } = [];
    Dictionary<string, AutomationTimer> ByScript { get; set; } = [];
    Dictionary<string, AutomationTimer> Named { get; set; } = [];
    Dictionary<string, AutomationTimer> Temporary { get; set; } = [];
    Dictionary<string, Dictionary<string, AutomationTimer>> Grouped { get; set; } = [];
    public EventHandler<Timer>? OnFire { get; set; }

    public void TimerCallback(Timer timer)
    {
        if (timer.OneShot)
        {
            _removeTimer(timer.ID);
        }
        OnFire!.Invoke(this, timer);
    }
    public bool AddTimer(Timer timer, bool replace)
    {
        lock (All)
        {
            var name = timer.PrefixedName();
            if (name != "")
            {
                var named = Named.TryGetValue(name, out var t) ? t : null;
                if (named is not null)
                {
                    if (!replace)
                    {
                        return false;
                    }
                    _removeTimer(named.Data.ID);
                }
            }
            return _addTimer(timer);
        }
    }
    public bool RemoveTimer(string id)
    {
        lock (All)
        {
            return _removeTimer(id);
        }
    }
    public bool RemoveTimerByName(string name)
    {
        lock (All)
        {
            var named = Named.TryGetValue(name, out var t) ? t : null;
            if (named is null)
            {
                return false;
            }
            return _removeTimer(named.Data.ID);
        }
    }
    public int DeleteTemporaryTimers()
    {
        lock (All)
        {
            var count = Temporary.Count;
            foreach (var t in Temporary.Values)
            {
                _removeTimer(t.Data.ID);
            }
            return count;
        }
    }
    public int DeleteTimerGroup(string group, bool byUser)
    {
        lock (All)
        {
            var count = 0;
            if (Grouped.TryGetValue(group, out var groupTimers))
            {
                foreach (var t in groupTimers.Values)
                {
                    if (byUser == t.Data.ByUser())
                    {
                        count++;
                        _removeTimer(t.Data.ID);
                    }

                }
            }
            return count;
        }
    }
    public bool EnableTimerByName(string name, bool enable)
    {
        lock (All)
        {
            var named = Named.TryGetValue(name, out var t) ? t : null;
            if (named is null)
            {
                return false;
            }
            if (enable)
            {
                named.Start();
            }
            else
            {
                named.Stop();
            }
            return true;
        }
    }
    public int EnableTimerGroup(string group, bool enabled)
    {
        lock (All)
        {
            var count = 0;
            if (Grouped.TryGetValue(group, out var groupTimers))
            {
                count = groupTimers.Count;
                foreach (var t in groupTimers.Values)
                {
                    if (enabled)
                    {
                        t.Start();
                    }
                    else
                    {
                        t.Stop();
                    }
                }
            }
            return count;
        }
    }
    public List<string> ListTimerNames(bool byUser)
    {
        lock (All)
        {
            var list = new List<string>();
            foreach (var t in All.Values)
            {
                if (byUser == t.Data.ByUser())
                {
                    list.Add(t.Data.Name);
                }
            }
            return list;
        }
    }
    public bool HasNamedTimer(string name)
    {
        return Named.ContainsKey(name);
    }
    public bool ResetNamedTimer(string name)
    {
        lock (All)
        {
            var named = Named.TryGetValue(name, out var t) ? t : null;
            if (named is null)
            {
                return false;
            }
            if (!named.Data.Enabled)
            {
                return false;
            }
            named.Reset();
            return true;
        }
    }
    public void ResetTimers()
    {
        lock (All)
        {
            foreach (var t in All.Values)
            {
                if (t.Data.Enabled)
                {
                    t.Reset();
                }
            }
        }
    }
    public FoundStringResult GetTimerOption(string name, string option)
    {
        lock (All)
        {
            var named = Named.TryGetValue(name, out var t) ? t : null;
            if (named is null)
            {
                return FoundStringResult.NotFound;
            }
            return named.Option(option).Found();
        }
    }
    public FoundStringResult GetTimerInfo(string name, int infotype)
    {
        lock (All)
        {
            var named = Named.TryGetValue(name, out var t) ? t : null;
            if (named is null)
            {
                return FoundStringResult.NotFound;
            }
            return named.Info(infotype).Found();
        }
    }
    public FoundBoolResult SetTimerOption(string name, string option, string value)
    {
        lock (All)
        {
            var named = Named.TryGetValue(name, out var t) ? t : null;
            if (named is null)
            {
                return FoundBoolResult.NotFound;
            }
            return named.SetOption(option, value).Found();
        }
    }
    public List<Timer> GetTimersByType(bool byuser)
    {
        lock (All)
        {
            Dictionary<string, AutomationTimer> timers = byuser ? ByUser : ByScript;
            return timers.Values.Select(t => t.Data).ToList();
        }
    }
    public void AddTimers(List<Timer> timers)
    {
        lock (All)
        {
            foreach (var t in timers)
            {
                _addTimer(t);
            }
        }
    }
    public void DoDeleteTimerByType(bool byuser)
    {
        lock (All)
        {
            var list = byuser ? ByUser.Values.ToList() : ByScript.Values.ToList();
            foreach (var t in list)
            {
                _removeTimer(t.Data.ID);
            }
        }
    }
    public Timer? GetTimer(string id)
    {
        lock (All)
        {
            var t = All.TryGetValue(id, out var timer) ? timer : null;
            return t?.Data;
        }
    }
    public int DoUpdateTimer(Timer timer)
    {
        lock (All)
        {
            var old = All.TryGetValue(timer.ID, out var existing) ? existing : null;
            if (old is null)
            {
                return MushString.UpdateFailNotFound;
            }
            timer.SetByUser(old.Data.ByUser());
            if (timer.Name != "" && timer.Name != old.Data.Name && Named.ContainsKey(timer.PrefixedName()))
            {
                return MushString.UpdateFailDuplicateName;
            }
            _unloadTimer(old.Data.ID);
            old.Data = timer;
            _loadTimer(old);
            return MushString.UpdateOK;
        }
    }
    public void Flush()
    {
        lock (All)
        {
            var list = All.Values.ToList();
            foreach (var t in list)
            {
                _removeTimer(t.Data.ID);
            }
        }
    }
    public bool _addTimer(Timer timer)
    {
        if (All.ContainsKey(timer.ID))
        {
            return false;
        }
        var t = _createTimer(timer);
        _loadTimer(t);
        return true;
    }

    public AutomationTimer _createTimer(Timer timer)
    {
        return new AutomationTimer(timer, TimerCallback);
    }
    private void _loadTimer(AutomationTimer timer)
    {
        var t = timer.Data;
        All[t.ID] = timer;
        if (!t.Temporary)
        {
            if (t.ByUser())
            {
                ByUser[t.ID] = timer;
            }
            else
            {
                ByScript[t.ID] = timer;
            }
        }
        if (t.Name != "")
        {
            Named[t.PrefixedName()] = timer;
        }
        if (t.Group != "")
        {
            if (!Grouped.ContainsKey(t.Group))
            {
                Grouped[t.Group] = [];
            }
            Grouped[t.Group][t.ID] = timer;
        }
        if (t.Temporary)
        {
            Temporary[t.ID] = timer;
        }
        if (t.Enabled)
        {
            timer.Start();
        }
    }
    private bool _removeTimer(string id)
    {
        var t = _unloadTimer(id);
        return t is not null;
    }
    public AutomationTimer? _unloadTimer(string id)
    {
        var t = All.TryGetValue(id, out var timer) ? timer : null;
        if (t is null)
        {
            return null;
        }
        All.Remove(id);
        if (t.Data.Name != "")
        {
            Named.Remove(t.Data.Name);
        }
        if (t.Data.Group != "")
        {
            if (Grouped.TryGetValue(t.Data.Group, out var group))
            {
                group.Remove(t.Data.ID);
                if (group.Count == 0)
                {
                    Grouped.Remove(t.Data.Group);
                }
            }
        }
        if (t.Data.Temporary)
        {
            Temporary.Remove(t.Data.ID);
        }
        t.Stop();
        return t;
    }
}

