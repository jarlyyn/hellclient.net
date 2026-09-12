namespace Hellclient.World.Infras.Components;

using Hellclient.World.Infras.Adapters;
using Hellclient.World.Types;
internal class Rooms
{
    public Dictionary<string, Room> rooms { get; set; } = new();
    public Dictionary<string, List<WalkPath>> temporaryPaths { get; set; } = new();
    public Room? GetRoom(string id)
    {
        return rooms.ContainsKey(id) ? rooms[id] : null;
    }
    public List<WalkPath>? GetTemporaryPaths(string id)
    {
        return temporaryPaths.ContainsKey(id) ? temporaryPaths[id] : null;
    }
    public List<string> GetRoomID(string name)
    {
        var result = new List<String>();
        foreach (var v in rooms.Values)
        {
            if (v.Name == name)
            {
                result.Add(v.ID);
            }
        }
        return result;
    }
    public string GetRoomName(string id)
    {
        return rooms.ContainsKey(id) ? rooms[id].Name : string.Empty;
    }
    public void SetRoomName(string id, string name)
    {
        if (!rooms.ContainsKey(id))
        {
            var room = new Room();
            room.ID = id;
            rooms[id] = room;
        }
        rooms[id].Name = name;

    }
    public bool AddPath(string id, WalkPath p)
    {
        if (!rooms.ContainsKey(id))
        {
            return false;
        }
        rooms[id].Exits.Add(p);
        return true;
    }
    public bool AddTemporaryPath(string id, WalkPath p)
    {
        if (!temporaryPaths.ContainsKey(id))
        {
            temporaryPaths[id] = new();
        }
        temporaryPaths[id].Add(p);
        return true;
    }
    public void ClearRoom(string id)
    {
        if (rooms.ContainsKey(id))
        {
            rooms.Remove(id);
        }
    }
    public List<string> NewArea(int size)
    {
        var result = new List<string>();
        for (int i = 0; i < size; i++)
        {
            var id = SimpleID.Instance.GenerateID();
            var room = new Room();
            room.ID = id;
            rooms[id] = room;
            result.Add(id);
        }
        return result;
    }
    public List<WalkPath> GetExits(string id, Dictionary<string, bool> tags, bool all)
    {
        var result = new List<WalkPath>();
        var room = rooms.TryGetValue(id, out var r) ? r : null;
        var texits = temporaryPaths.TryGetValue(id, out var t) ? t : null;
        if (room is null && texits is null)
        {
            return result;
        }
        if (room is not null)
        {
            foreach (var exit in room.Exits)
            {
                if (all || Mapper.ValidateTags(tags, exit))
                {
                    result.Add(exit);
                }
            }
        }
        if (texits is not null)
        {
            foreach (var exit in texits)
            {
                if (all || Mapper.ValidateTags(tags, exit))
                {
                    result.Add(exit);
                }
            }
        }
        return result;
    }
    public void Reset()
    {
        rooms = new();
        temporaryPaths = new();
    }
    public void ResetTemporary()
    {
        temporaryPaths = new();
    }
}

public class Option
{
    public List<string> Blacklist { get; set; } = new();
    public List<string> Whitelist { get; set; } = new();
    public List<List<string>> BlockedPath { get; set; } = new();
}
public class Step
{
    public static Step EmptyStep { get; } = new Step();
    public string To { get; set; } = "";
    public string From { get; set; } = "";
    public string Command { get; set; } = "";
    public int Delay { get; set; } = 0;
    public int remain { get; set; } = 0;
}
class Walking
{
    public Dictionary<string, bool> tags = new();
    public Rooms rooms = new();
    public string from = "";
    public List<string> to = new();
    public List<WalkPath> fly = new();
    public Dictionary<string, Step> walked = new();
    public List<Step> forwarding = new();
    public Dictionary<string, bool> blacklist = new();
    public Dictionary<string, bool> whitelist = new();
    public Dictionary<string, Dictionary<string, bool>> blockedpath = new();
    public int maxdistance = 0;
    private Step step(WalkPath p)
    {
        var length = p.Delay;
        if (length < 1)
        {
            length = 1;
        }
        return new Step
        {
            To = p.To,
            From = p.From,
            Command = p.Command,
            Delay = length,
            remain = length
        };
    }
    public Step FlyStep(WalkPath p)
    {
        var step = this.step(p);
        step.From = from;
        return step;
    }
    private bool validateExit(WalkPath p)
    {
        if (blacklist.ContainsKey(p.To) && blacklist[p.To])
        {
            return false;
        }
        if (whitelist.Count > 0 && (!whitelist.ContainsKey(p.To) || !whitelist[p.To]))
        {
            return false;
        }
        if (blockedpath.ContainsKey(p.From) && blockedpath[p.From].ContainsKey(p.To) && blockedpath[p.From][p.To])
        {
            return false;
        }
        return Mapper.ValidateTags(tags, p);
    }
    public List<Step>? Walk()
    {
        var distance = 0;
        var tolist = new Dictionary<string, bool>();
        var froom = rooms.GetRoom(from);
        var ftexits = rooms.GetTemporaryPaths(from);
        if (froom == null && ftexits == null)
        {
            return null;
        }
        walked[from] = Step.EmptyStep;
        if (froom is not null)
        {
            foreach (var ve in froom.Exits)
            {
                if (!walked.ContainsKey(ve.To) && validateExit(ve))
                {
                    forwarding.Append(this.step(ve));
                }
            }
        }
        if (ftexits is not null)
        {
            foreach (var ve in ftexits)
            {
                if (!walked.ContainsKey(ve.To) && validateExit(ve))
                {
                    forwarding.Append(this.step(ve));
                }
            }
        }
        foreach (var vf in fly)
        {
            if (!walked.ContainsKey(vf.To) && validateExit(vf))
            {
                forwarding.Append(FlyStep(vf));
            }
        }
        if (forwarding.Count == 0)
        {
            return null;
        }
        foreach (var vt in to)
        {

            if (rooms.GetRoom(vt) == null && rooms.GetTemporaryPaths(vt) == null)
            {
                continue;
            }
            if (from == vt)
            {
                return new List<Step>();
            }
            tolist[vt] = true;

        }
        if (tolist.Count == 0)
        {
            return null;
        }
        var matchedRoom = "";
        while (true)
        {
            var newExits = new List<Step>();
            distance++;
            if (maxdistance > 0 && distance > maxdistance)
            {
                break;

            }
            while (true)
            {
                if (forwarding.Count == 0)
                {
                    break;
                }
                var fstep = forwarding[0];
                forwarding.RemoveAt(0);
                var room = rooms.GetRoom(fstep.To);
                var texits = rooms.GetTemporaryPaths(fstep.To);
                if (!walked.ContainsKey(fstep.To) || (room is null && texits is null))
                {
                    continue;

                }
                if (maxdistance > 0 && fstep.Delay > maxdistance)
                {
                    continue;
                }
                fstep.remain--;
                if (fstep.remain > 0)
                {
                    newExits.Add(fstep);
                    continue;
                }
                walked[fstep.To] = fstep;
                if (tolist.ContainsKey(fstep.To) && tolist[fstep.To])
                {
                    matchedRoom = fstep.To;

                    goto Matching;
                }
                if (room is not null)
                {
                    foreach (var exit in room.Exits)
                    {
                        if (!walked.ContainsKey(exit.To) && validateExit(exit))
                        {
                            newExits.Add(this.step(exit));
                        }
                    }
                }
                if (texits is not null)
                {
                    foreach (var exit in texits)
                    {
                        if (!walked.ContainsKey(exit.To) && validateExit(exit))
                        {
                            newExits.Add(this.step(exit));
                        }
                    }
                }

            }
            forwarding.AddRange(newExits);
            if (forwarding.Count == 0)
            {
                goto Matching;
            }
        }
    Matching:

        if (matchedRoom == "")
        {
            return null;
        }
        var result = new List<Step>();
        var step = walked[matchedRoom];
        for (; ; )
        {
            if (step == null || step == Step.EmptyStep)
            {
                break;
            }
            result.Insert(0, step);

            step = walked[step.From];

        }
        return result;
    }
    public static Walking New(Option? opt)
    {
        var walking = new Walking();
        if (opt is not null)
        {
            foreach (var v in opt.Blacklist)
            {
                walking.blacklist[v] = true;
            }
            foreach (var v in opt.Whitelist)
            {
                walking.whitelist[v] = true;
            }
            foreach (var v in opt.BlockedPath)
            {
                if (v.Count == 2)
                {
                    if (!walking.blockedpath.ContainsKey(v[0]))
                    {
                        walking.blockedpath[v[0]] = new();
                    }
                    walking.blockedpath[v[0]][v[1]] = true;
                }
            }
        }
        return walking;
    }
}

public class WalkAllResult
{
    public List<Step> Steps { get; set; } = new();
    public List<string> Walked { get; set; } = new();
    public List<string> NotWalked { get; set; } = new();
}
class WalkAll
{
    public List<string> Targets { get; set; } = new();
    public int MaxDistance { get; set; }
    public Rooms Rooms { get; set; } = new();
    public Dictionary<string, bool> Tags { get; set; } = new();
    public List<WalkPath> Fly { get; set; } = new();
    public Option Option { get; set; } = new();
    public List<Step>? Walk(string fr, List<string> to)
    {
        var w = NewWalking();
        w.from = fr;
        w.to = to;
        w.maxdistance = MaxDistance;
        return w.Walk();
    }
    private Walking NewWalking()
    {
        var w = Walking.New(Option);
        w.rooms = Rooms;
        w.tags = Tags;
        w.fly = Fly;
        return w;
    }
    List<string> filter(List<string> inlist, string filtered)
    {
        var result = new List<string>();
        foreach (var v in inlist)
        {
            if (v != filtered)
            {
                result.Add(v);
            }
        }
        return result;
    }
    public WalkAllResult? Start()
    {
        var result = new WalkAllResult();
        if (Targets.Count < 2)
        {
            return null;
        }
        var fr = Targets[0];
        var left = Targets[1..];
        result.Walked.Add(fr);
        while (left.Count > 0)
        {
            var steps = Walk(fr, left);
            if (steps == null || steps.Count == 0)
            {
                break;
            }
            result.Steps.AddRange(steps);
            fr = steps.Last().To;
            left = filter(left, fr);
            result.Walked.Add(fr);
        }
        result.NotWalked.AddRange(left);
        return result;
    }
}
public class Mapper
{
    public static bool ValidateTags(Dictionary<string, bool> tags, WalkPath p)
    {
        var matched = false;
        foreach (var kvp in tags)
        {
            var k = kvp.Key;
            var v = kvp.Value;
            if (v)
            {
                if (p.ExcludeTags.ContainsKey(k) && p.ExcludeTags[k])
                {
                    return false;
                }
                if (p.Tags.ContainsKey(k) && p.Tags[k])
                {
                    matched = true;
                }
            }
        }
        return p.Tags.Count == 0 || matched;
    }
    private Rooms Rooms { get; set; } = new();
    private Dictionary<string, bool> tags = new();
    private List<WalkPath> fly = new();

    public List<WalkPath> FlyList()
    {
        return fly;
    }
    public void SetFlyList(List<WalkPath> newFlyList)
    {
        fly = newFlyList;
    }
    public void FlushTags()
    {
        tags.Clear();
    }
    public void AddTags(List<string> tags)
    {
        foreach (var tag in tags)
        {
            this.tags[tag] = true;
        }
    }
    public void SetTag(string tag, bool enabled)
    {
        if (enabled)
        {
            tags[tag] = true;
        }
        else
        {
            tags.Remove(tag);
        }
    }
    public List<string> Tags()
    {
        var result = new List<string>();
        foreach (var v in tags)
        {
            if (v.Value)
            {
                result.Add(v.Key);
            }
        }
        return result;
    }
    public WalkAllResult? WalkAll(List<string> targets, bool fly, int max_distance, Option option)
    {
        var a = new WalkAll() { Option = option };
        a.Rooms = Rooms;
        a.Tags = tags;
        a.Fly = fly ? this.fly : new();
        a.Targets = targets;
        a.MaxDistance = max_distance;
        return a.Start();
    }
    Walking NewWalking(Option option)
    {
        var walking = Walking.New(option);
        walking.rooms = Rooms;
        walking.tags = tags;
        walking.fly = fly;
        return walking;
    }
    public List<Step>? GetPath(string from, bool fly, List<string> to, Option option)
    {
        var w = NewWalking(option);
        w.from = from;
        w.to = to;
        w.fly = fly ? this.fly : new();
        return w.Walk();
    }
    public bool RemoveRoom(string id)
    {
        if (!Rooms.rooms.ContainsKey(id))
        {
            return false;
        }
        Rooms.rooms.Remove(id);
        return true;
    }
    public string GetRoomName(string id)
    {
        return Rooms.GetRoomName(id);
    }
    public void SetRoomName(string id, string name)
    {
        Rooms.SetRoomName(id, name);
    }
    public bool AddPath(string id, WalkPath path)
    {
        return Rooms.AddPath(id, path);
    }
    public bool AddTemporaryPath(string id, WalkPath path)
    {
        return Rooms.AddTemporaryPath(id, path);
    }
    public void ClearRoom(string id)
    {
        Rooms.ClearRoom(id);
    }
    public List<string> NewArea(int size)
    {
        return Rooms.NewArea(size);
    }
    public List<WalkPath> GetExits(string id, bool all)
    {
        return Rooms.GetExits(id, tags, all);
    }
    public void Reset()
    {
        Rooms.Reset();
    }
    public void ResetTemporary()
    {
        Rooms.ResetTemporary();
    }
}