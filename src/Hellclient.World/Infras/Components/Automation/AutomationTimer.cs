namespace Hellclient.World.Components.Automation;

using Hellclient.World.Types;
using Hellclient.World.Utils;

public class AutomationTimer
{
    public AutomationTimer(Timer timer, Action<Timer> onFire)
    {
        Data = timer;
        OnFire = onFire;
    }
    public Timer Data { get; set; } = new Timer();
    private System.Timers.Timer? _timer { get; set; } = null;

    public Action<Timer> OnFire { get; init; }

    private void _onFire()
    {
        lock (Data)
        {
            OnFire?.Invoke(Data);
        }

    }
    private void _start()
    {
        if (_timer == null)
        {
            _timer = new System.Timers.Timer(Data.GetDuration().TotalMilliseconds)
            {
                AutoReset = true
            };
            _timer.Elapsed += (sender, e) =>
            {
                _onFire();
            };
        }
    }
    public void Start()
    {
        lock (Data)
        {
            _start();
            _timer?.Start();
        }
    }
    private void _stop()
    {
        if (_timer != null)
        {
            _timer.Stop();
            _timer.Dispose();
            _timer = null;
        }
    }
    public void Stop()
    {
        lock (Data)
        {
            _stop();
        }
    }
    public void Reset()
    {
        lock (Data)
        {
            if (_timer is not null)
            {
                _stop();
                _start();
            }
        }
    }
    public StringResult Info(int infotype)
    {
        lock (Data)
        {
            return infotype switch
            {
                1 => new StringResult(Data.Hour.ToString(), true),
                2 => new StringResult(Data.Minute.ToString(), true),
                3 => new StringResult(Data.Second.ToString("F"), true),
                4 => new StringResult(Data.Send, true),
                5 => new StringResult(Data.Script, true),
                6 => new StringResult(MushString.ToStringBool(Data.Enabled), true),
                7 => new StringResult(MushString.ToStringBool(Data.OneShot), true),
                8 => new StringResult(MushString.ToStringBool(Data.AtTime), true),
                14 => new StringResult(MushString.ToStringBool(Data.Temporary), true),
                19 => new StringResult(Data.Group, true),
                20 => new StringResult(Data.SendTo.ToString(), true),
                21 => new StringResult("0", true),
                22 => new StringResult(Data.Name, true),
                23 => new StringResult(MushString.ToStringBool(Data.OmitFromOutput), true),
                24 => new StringResult(MushString.ToStringBool(Data.OmitFromLog), true),
                _ => new StringResult(string.Empty, false),
            };
        }
    }

    public StringResult Option(string name)
    {
        lock (Data)
        {
            return name switch
            {
                "active_closed" => new StringResult(MushString.ToStringBool(Data.ActionWhenDisconnectd), true),
                "at_time" => new StringResult(MushString.ToStringBool(Data.AtTime), true),
                "enabled" => new StringResult(MushString.ToStringBool(Data.Enabled), true),
                "group" => new StringResult(Data.Group, true),
                "hour" => new StringResult(Data.Hour.ToString(), true),
                "minute" => new StringResult(Data.Minute.ToString(), true),
                "name" => new StringResult(Data.Name, true),
                "offset_hour" => new StringResult("0", true),
                "offset_minute" => new StringResult("0", true),
                "offset_second" => new StringResult("0", true),
                "omit_from_log" => new StringResult(MushString.ToStringBool(Data.OmitFromLog), true),
                "omit_from_output" => new StringResult(MushString.ToStringBool(Data.OmitFromOutput), true),
                "one_shot" => new StringResult(MushString.ToStringBool(Data.OneShot), true),
                "script" => new StringResult(Data.Script, true),
                "second" => new StringResult(Data.Second.ToString("F"), true),
                "send" => new StringResult(Data.Send, true),
                "send_to" => new StringResult(Data.SendTo.ToString(), true),
                "user" => new StringResult("0", true),
                "variable" => new StringResult(Data.Variable, true),
                _ => new StringResult(string.Empty, false),
            };
        }
    }

    public BoolResult SetOption(string name, string val)
    {
        lock (Data)
        {
            switch (name)
            {
                case "active_closed":
                    Data.ActionWhenDisconnectd = MushString.FromStringBool(val);
                    return new BoolResult(true, true);
                case "at_time":
                    Data.AtTime = MushString.FromStringBool(val);
                    return new BoolResult(true, true);
                case "enabled":
                    Data.Enabled = MushString.FromStringBool(val);
                    return new BoolResult(true, true);
                case "group":
                    Data.Group = val;
                    return new BoolResult(true, true);
                case "hour":
                    if (!int.TryParse(val, out var hour))
                    {
                        return new BoolResult(false, true);
                    }

                    Data.Hour = hour;
                    return new BoolResult(true, true);
                case "minute":
                    Data.Minute = int.TryParse(val, out var minute) ? minute : 0;
                    return new BoolResult(true, true);
                case "offset_hour":
                case "offset_minute":
                case "offset_second":
                    return new BoolResult(false, false);
                case "omit_from_log":
                    Data.OmitFromLog = MushString.FromStringBool(val);
                    return new BoolResult(true, true);
                case "omit_from_output":
                    Data.OmitFromOutput = MushString.FromStringBool(val);
                    return new BoolResult(true, true);
                case "one_shot":
                    Data.OneShot = MushString.FromStringBool(val);
                    return new BoolResult(true, true);
                case "script":
                    Data.Script = val;
                    return new BoolResult(true, true);
                case "second":
                    Data.Second = double.TryParse(val, out var second) ? second : 0;
                    return new BoolResult(true, true);
                case "send":
                    Data.Send = val;
                    return new BoolResult(true, true);
                case "send_to":
                    Data.SendTo = int.TryParse(val, out var sendTo) ? sendTo : 0;
                    return new BoolResult(true, true);
                case "user":
                    return new BoolResult(false, false);
                case "variable":
                    Data.Variable = val;
                    return new BoolResult(true, true);
                default:
                    return new BoolResult(false, false);
            }
        }
    }

}
