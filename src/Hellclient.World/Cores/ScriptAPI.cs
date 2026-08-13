namespace Hellclient.World.Cores;

using Hellclient.World.Components.Automation;
using Hellclient.World.Configs;
using Hellclient.World.Helpers;
using Hellclient.World.Infras.Adapters;
using Hellclient.World.Infras.Components;
using Hellclient.World.Types;
using Hellclient.World.Utils;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
//统一对外的标准API实现
public class ScriptAPI(IWorld world)
{
    private int uniqueNumber = 0;
    public const int EOK = 0;                             //No error
    public const int EWorldOpen = 30001;                  //The world is already open
    public const int EWorldClosed = 30002;                //The world is closed, this action cannot be performed
    public const int ENoNameSpecified = 30003;            //No name has been specified where one is required
    public const int ECannotPlaySound = 30004;            //The sound file could not be played
    public const int ETriggerNotFound = 30005;            //The specified trigger name does not exist
    public const int ETriggerAlreadyExists = 30006;       //Attempt to add a trigger that already exists
    public const int ETriggerCannotBeEmpty = 30007;       //The trigger "match" string cannot be empty
    public const int EInvalidObjectLabel = 30008;         //The name of this object is invalid
    public const int EScriptNameNotLocated = 30009;       //Script name is not in the script file
    public const int EAliasNotFound = 30010;              //The specified alias name does not exist
    public const int EAliasAlreadyExists = 30011;         //Attempt to add a alias that already exists
    public const int EAliasCannotBeEmpty = 30012;         //The alias "match" string cannot be empty
    public const int ECouldNotOpenFile = 30013;           //Unable to open requested file
    public const int ELogFileNotOpen = 30014;             //Log file was not open
    public const int ELogFileAlreadyOpen = 30015;         //Log file was already open
    public const int ELogFileBadWrite = 30016;            //Bad write to log file
    public const int ETimerNotFound = 30017;              //The specified timer name does not exist
    public const int ETimerAlreadyExists = 30018;         //Attempt to add a timer that already exists
    public const int EVariableNotFound = 30019;           //Attempt to delete a variable that does not exist
    public const int ECommandNotEmpty = 30020;            //Attempt to use SetCommand with a non-empty command window
    public const int EBadRegularExpression = 30021;       //Bad regular expression syntax
    public const int ETimeInvalid = 30022;                //Time given to AddTimer is invalid
    public const int EBadMapItem = 30023;                 //Direction given to AddToMapper is invalid
    public const int ENoMapItems = 30024;                 //No items in mapper
    public const int EUnknownOption = 30025;              //Option name not found
    public const int EOptionOutOfRange = 30026;           //New value for option is out of range
    public const int ETriggerSequenceOutOfRange = 30027;  //Trigger sequence value invalid
    public const int ETriggerSendToInvalid = 30028;       //Where to send trigger text to is invalid
    public const int ETriggerLabelNotSpecified = 30029;   //Trigger label not specified/invalid for 'send to variable'
    public const int EPluginFileNotFound = 30030;         //File name specified for plugin not found
    public const int EProblemsLoadingPlugin = 30031;      //There was a parsing or other problem loading the plugin
    public const int EPluginCannotSetOption = 30032;      //Plugin is not allowed to set this option
    public const int EPluginCannotGetOption = 30033;      //Plugin is not allowed to get this option
    public const int ENoSuchPlugin = 30034;               //Requested plugin is not installed
    public const int ENotAPlugin = 30035;                 //Only a plugin can do this
    public const int ENoSuchRoutine = 30036;              //Plugin does not support that subroutine (subroutine not in script)
    public const int EPluginCouldNotSaveState = 30037;    //Plugin could not save state (eg. no state directory)
    public const int EPluginDoesNotSaveState = 30038;     //Plugin does not support saving state
    public const int EPluginDisabled = 30039;             //Plugin is currently disabled
    public const int EErrorCallingPluginRoutine = 30040;  //Could not call plugin routine
    public const int ECommandsNestedTooDeeply = 30041;    //Calls to "Execute" nested too deeply
    public const int ECannotCreateChatSocket = 30042;     //Unable to create socket for chat connection
    public const int ECannotLookupDomainName = 30043;     //Unable to do DNS (domain name) lookup for chat connection
    public const int ENoChatConnections = 30044;          //No chat connections open
    public const int EChatPersonNotFound = 30045;         //Requested chat person not connected
    public const int EBadParameter = 30046;               //General problem with a parameter to a script call
    public const int EChatAlreadyListening = 30047;       //Already listening for incoming chats
    public const int EChatIDNotFound = 30048;             //Chat session with that ID not found
    public const int EChatAlreadyConnected = 30049;       //Already connected to that server/port
    public const int EClipboardEmpty = 30050;             //Cannot get (text from the) clipboard
    public const int EFileNotFound = 30051;               //Cannot open the specified file
    public const int EAlreadyTransferringFile = 30052;    //Already transferring a file
    public const int ENotTransferringFile = 30053;        //Not transferring a file
    public const int ENoSuchCommand = 30054;              //There is not a command of that name
    public const int EArrayAlreadyExists = 30055;         //That array already exists
    public const int EArrayDoesNotExist = 30056;          //That array does not exist
    public const int EBadKeyName = 30056;                //That name is not permitted for a key
    public const int EArrayNotEvenNumberOfValues = 30057; //Values to be imported into array are not in pairs
    public const int EImportedWithDuplicates = 30058;     //Import succeeded, however some values were overwritten
    public const int EBadDelimiter = 30059;               //Import/export delimiter must be a single character, other than backslash
    public const int ESetReplacingExistingValue = 30060;  //Array element set, existing value overwritten
    public const int EKeyDoesNotExist = 30061;            //Array key does not exist
    public const int ECannotImport = 30062;               //Cannot import because cannot find unused temporary character
    public const int EItemInUse = 30063;                  //Cannot delete trigger/alias/timer because it is executing a script
    public const int ESpellCheckNotActive = 30064;        //Spell checker is not active
    public const int ECannotAddFont = 30065;              //Cannot create requested font
    public const int EPenStyleNotValid = 30066;           //Invalid settings for pen parameter
    public const int EUnableToLoadImage = 30067;          //Bitmap image could not be loaded
    public const int EImageNotInstalled = 30068;          //Image has not been loaded into window
    public const int EInvalidNumberOfPoints = 30069;      //Number of points supplied is incorrect
    public const int EInvalidPoint = 30070;               //Point is not numeric
    public const int EHotspotPluginChanged = 30071;       //Hotspot processing must all be in same plugin
    public const int EHotspotNotInstalled = 30072;        //Hotspot has not been defined for this window
    public const int ENoSuchWindow = 30073;               //Requested miniwindow does not exist
    public const int EBrushStyleNotValid = 30074;         //Invalid settings for brush parameter
    public IWorld World { get; init; } = world;
    public string Version() => AppVersion.Version.FullVersionCode();
    public void Note(string message) => World.DoPrint(message);
    public void PrintSystem(string msg)
    {
        World.DoPrintSystem(msg);
    }
    public int SendImmediate(string message)
    {
        var cmd = Command.Create(message);
        var caller = World.GetScriptCaller();
        cmd.Creator = caller.Creator;
        cmd.CreatorType = caller.CreatorType;
        World.DoMetronomeSend(cmd);
        return EOK;
    }
    public int Send(string message)
    {
        var cmd = Command.Create(message);
        var caller = World.GetScriptCaller();
        cmd.Creator = caller.Creator;
        cmd.CreatorType = caller.CreatorType;
        World.DoMetronomeSend(cmd);
        return EOK;
    }
    public int SendNoEcho(string message)
    {
        var cmd = Command.Create(message);
        var caller = World.GetScriptCaller();
        cmd.Creator = caller.Creator;
        cmd.CreatorType = caller.CreatorType;
        cmd.Echo = false;
        World.DoMetronomeSend(cmd);
        return EOK;
    }
    public int SendPush(string message)
    {
        var cmd = Command.Create(message);
        cmd.History = true;
        World.DoMetronomeSend(cmd);
        return EOK;
    }
    public int SendSpecial(string message, bool echo, bool queue, bool log, bool history)
    {
        var cmd = Command.Create(message);
        var caller = World.GetScriptCaller();
        cmd.Creator = caller.Creator;
        cmd.CreatorType = caller.CreatorType;
        cmd.Echo = echo;
        cmd.Log = log;
        cmd.History = history;
        if (queue)
        {
            World.DoSendToQueue(cmd);
        }
        else
        {
            World.DoMetronomeSend(cmd);
        }
        return EOK;
    }
    public int LogSend(string message)
    {
        var cmd = Command.Create(message);
        var caller = World.GetScriptCaller();
        cmd.Creator = caller.Creator;
        cmd.CreatorType = caller.CreatorType;
        cmd.Log = true;
        World.DoMetronomeSend(cmd);
        return EOK;
    }
    public int Execute(string message)
    {
        World.DoExecute(message);
        return EOK;
    }
    public int SendPkt(string packet)
    {
        return EOK;
    }

    public int Connect()
    {
        World.DoConnectServer();
        return EOK;
    }
    public bool IsConnected()
    {
        return World.GetConnConnected();
    }
    public int Disconnect()
    {
        World.DoCloseServer();
        return EOK;
    }
    public string Hash(string text)
    {
        var result = SHA1.HashData(Encoding.UTF8.GetBytes(text));
        return BitConverter.ToString(result[..]).Replace("-", "").ToLower();
    }
    public string Base64Encode(string text, bool mutliline)
    {
        var encoded = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        if (!mutliline)
        {
            return encoded;
        }
        var result = "";
        while (encoded.Length > 76)
        {
            result = result + encoded.Substring(0, 76) + "\n";
            encoded = encoded.Substring(76);
        }
        result = result + encoded;
        return result;
    }
    public string? Base64Decode(string text)
    {
        try
        {
            var decoded = System.Convert.FromBase64String(text);
            var result = Encoding.UTF8.GetString(decoded);
            return result;
        }
        catch
        {
            return null;
        }
    }
    public string GetVariable(string text)
    {
        return World.GetParam(text);
    }
    public int SetVariable(string name, string content)
    {
        World.SetParam(name, content);
        return EOK;
    }
    public int DeleteVariable(string name)
    {
        World.DeleteParam(name);
        return EOK;
    }
    public Dictionary<string, string> GetVariableList()
    {
        var allvar = World.GetParams();
        var result = new Dictionary<string, string>();
        foreach (var k in allvar.Keys)
        {
            result[k] = allvar[k];
        }
        return result;
    }
    public string GetVariableComment(string text)
    {
        return World.GetParamComment(text);
    }
    public int SetVariableComment(string name, string content)
    {
        World.SetParamComment(name, content);
        return EOK;
    }
    public int GetUniqueNumber()
    {
        var v = Interlocked.Add(ref uniqueNumber, 1);
        if (v < 0)
        {
            v = v + 2147483647;
        }
        return v;
    }
    public string GetUniqueID()
    {
        return SimpleID.Instance.GenerateID();
    }
    public string CreateGUID()
    {
        var uid = Guid.NewGuid();
        return uid.ToString().ToUpper();
    }

    public void SetStatus(string text)
    {
        World.SetStatus(text);
    }

    public object? GetWorldById(string WorldID)
    {
        return null;
    }

    public object? GetWorld(string WorldName)
    {
        return null;
    }

    public string GetWorldID()
    {
        return World.ID;
    }
    public List<string> GetWorldIdList()
    {
        return new List<string>();
    }
    public List<string> GetWorldList()
    {
        return new List<string>();
    }
    public string WorldName()
    {
        return World.ID;
    }
    public string WorldAddress()
    {
        return World.GetHost();
    }
    public int WorldPort()
    {
        int port;
        if (!int.TryParse(World.GetPort(), out port))
        {
            return 0;
        }
        return port;
    }
    public string WorldProxy()
    {
        return World.GetProxy();
    }
    public string Trim(string source)
    {
        return source.Trim();
    }

    public void FlashIcon() { }

    public List<string> GetQueue()
    {
        var cmds = World.GetQueue();
        var result = new List<string>(cmds.Count());
        for (int k = 0; k < cmds.Count(); k++)
        {
            result.Add(cmds[k].Message);
        }
        return result;
    }
    public int Queue(string message, bool echo)
    {
        var cmd = Command.Create(message);
        cmd.Echo = echo;
        var caller = World.GetScriptCaller();
        cmd.Creator = caller.Creator;
        cmd.CreatorType = caller.CreatorType;
        World.DoSendToQueue(cmd);
        return EOK;
    }
    public int DiscardQueue(bool force)
    {
        return World.DoDiscardQueue(force);
    }
    public void LockQueue()
    {
        World.DoLockQueue();
    }
    public int SpeedWalkDelay()
    {
        return World.GetQueueDelay();
    }
    public void SetSpeedWalkDelay(int d)
    {
        World.SetQueueDelay(d);
    }
    public void DeleteCommandHistory()
    {
        World.FlushHistories();
    }

    public int DoAfter(double seconds, string sendtext)
    {
        var t = Timer.Create();
        t.SetByUser(false);
        t.Enabled = true;
        t.OneShot = true;
        t.Second = seconds;
        t.SendTo = SendTo.SendtoWorld;
        t.Send = sendtext;
        t.Temporary = true;
        World.AddTimer(t, false);
        return EOK;
    }
    public int DoAfterNote(double seconds, string sendtext)
    {
        var t = Timer.Create();
        t.SetByUser(false);
        t.Enabled = true;
        t.OneShot = true;
        t.Second = seconds;
        t.SendTo = SendTo.SendtoOutput;
        t.Send = sendtext;
        t.Temporary = true;
        World.AddTimer(t, false);
        return EOK;
    }
    public int DoAfterSpeedWalk(double seconds, string sendtext)
    {
        var t = Timer.Create();
        t.SetByUser(false);
        t.Enabled = true;
        t.OneShot = true;
        t.Second = seconds;
        t.SendTo = SendTo.SendtoSpeedwalk;
        t.Send = sendtext;
        t.Temporary = true;
        World.AddTimer(t, false);
        return EOK;
    }

    public int DoAfterSpecial(double seconds, string sendtext, int sendto)
    {
        var t = Timer.Create();
        t.Enabled = true;
        t.OneShot = true;
        t.Second = seconds;
        t.SendTo = sendto;
        t.Send = sendtext;
        t.Temporary = true;
        World.AddTimer(t, false);
        return EOK;
    }

    public int DeleteGroup(string group)
    {
        return World.DoDeleteTriggerGroup(group, false) + World.DoDeleteTimerGroup(group, false) + World.DoDeleteAliasGroup(group, false);
    }
    public int AddTimer(string timerName, int hour, int minute, double second, string responseText, int flags, string scriptName)
    {
        var t = Timer.Create();
        t.Name = timerName;
        t.Hour = hour;
        t.Minute = minute;
        t.Second = second;
        t.Send = responseText;
        t.Script = scriptName;
        t.Enabled = (flags & Timer.TimerFlagEnabled) != 0;
        t.AtTime = (flags & Timer.TimerFlagAtTime) != 0;
        t.OneShot = (flags & Timer.TimerFlagOneShot) != 0;
        t.ActionWhenDisconnectd = (flags & Timer.TimerFlagActiveWhenClosed) != 0;
        t.Temporary = (flags & Timer.TimerFlagTemporary) != 0;
        t.SetByUser(false);
        World.AddTimer(t, (flags & Timer.TimerFlagReplace) != 0);
        return EOK;
    }
    public int DeleteTemporaryTimers()
    {
        return World.DoDeleteTemporaryTimers();
    }
    public int DeleteTimer(string name)
    {
        name = PrefixUtil.PrefixedName(name, false);
        if (!World.DoDeleteTimerByName(name))
        {
            return ETimerNotFound;
        }
        return EOK;
    }

    public int DeleteTimerGroup(string group)
    {
        return World.DoDeleteTimerGroup(group, false);
    }

    public int EnableTimer(string name, bool enabled)
    {
        name = PrefixUtil.PrefixedName(name, false);
        if (!World.DoEnableTimerByName(name, enabled))
        {
            return ETimerNotFound;
        }
        return EOK;
    }

    public int EnableTimerGroup(string group, bool enabled)
    {
        return World.DoEnableTimerGroup(group, enabled);
    }
    public List<string> GetTimerList()
    {
        return World.DoListTimerNames(false);
    }

    public int IsTimer(string name)
    {
        name = PrefixUtil.PrefixedName(name, false);
        if (!World.HasNamedTimer(name))
        {
            return ETimerNotFound;
        }
        return EOK;
    }
    public int ResetTimer(string name)
    {
        name = PrefixUtil.PrefixedName(name, false);
        if (!World.DoResetNamedTimer(name))
        {
            return ETimerNotFound;
        }
        return EOK;
    }
    public void ResetTimers()
    {
        World.DoResetTimers();
    }

    public (string, int) GetTimerOption(string name, string option)
    {
        name = PrefixUtil.PrefixedName(name, false);
        var (result, ofound, tfound) = World.GetTimerOption(name, option);
        if (!tfound)
        {
            return ("", ETimerNotFound);
        }
        if (!ofound)
        {
            return ("", EOptionOutOfRange);
        }
        return (result, EOK);
    }
    public int SetTimerOption(string name, string option, string value)
    {
        name = PrefixUtil.PrefixedName(name, false);
        var (result, ofound, tfound) = World.SetTimerOption(name, option, value);
        if (!tfound)
        {
            return ETimerNotFound;
        }
        if (!ofound)
        {
            return EOK;
        }
        if (!result)
        {
            return ETimeInvalid;
        }
        return EOK;
    }

    public int AddAlias(string aliasName, string match, string responseText, int flags, string scriptName)
    {
        if (match == "")
        {
            return EAliasCannotBeEmpty;
        }
        var alias = Alias.Create();
        alias.Name = aliasName;
        alias.Match = match;
        alias.Send = responseText;
        alias.Script = scriptName;
        alias.Enabled = (flags & Alias.AliasFlagEnabled) != 0;
        alias.KeepEvaluating = (flags & Alias.AliasFlagKeepEvaluating) != 0;
        alias.IgnoreCase = (flags & Alias.AliasFlagIgnoreAliasCase) != 0;
        alias.OmitFromLog = (flags & Alias.AliasFlagOmitFromLogFile) != 0;
        alias.Regexp = (flags & Alias.AliasFlagRegularExpression) != 0;
        alias.ExpandVariables = (flags & Alias.AliasFlagExpandVariables) != 0;
        alias.Temporary = (flags & Alias.AliasFlagTemporary) != 0;
        if ((flags & Alias.AliasFlagAliasSpeedWalk) != 0)
        {
            alias.SendTo = SendTo.SendtoSpeedwalk;
        }
        if ((flags & Alias.AliasFlagAliasQueue) != 0)
        {
            alias.SendTo = SendTo.SendtoCommandqueue;
        }
        alias.Menu = (flags & Alias.AliasFlagAliasMenu) != 0;
        alias.SetByUser(false);
        var ok = World.AddAlias(alias, (flags & Alias.AliasFlagReplace) != 0);
        if (!ok)
        {
            return EAliasAlreadyExists;
        }
        return EOK;
    }

    public int DeleteAlias(string name)
    {
        name = PrefixUtil.PrefixedName(name, false);
        var ok = World.DoDeleteAliasByName(name);
        if (!ok)
        {
            return EAliasNotFound;
        }
        return EOK;
    }
    public int DeleteAliasGroup(string group)
    {
        return World.DoDeleteAliasGroup(group, false);
    }
    public int DeleteTemporaryAliases()
    {
        return World.DoDeleteTemporaryAliases();
    }
    public int EnableAlias(string name, bool enabled)
    {
        name = PrefixUtil.PrefixedName(name, false);
        var ok = World.DoEnableAliasByName(name, enabled);
        if (!ok)
        {
            return EAliasNotFound;
        }
        return EOK;
    }

    public int EnableAliasGroup(string group, bool enabled)
    {
        return World.DoEnableAliasGroup(group, enabled);
    }

    public List<string> GetAliasList()
    {
        return World.DoListAliasNames(false);
    }

    public (string, int) GetAliasOption(string name, string option)
    {
        name = PrefixUtil.PrefixedName(name, false);
        var (result, ofound, tfound) = World.GetAliasOption(name, option);
        if (!tfound)
        {
            return ("", ETimerNotFound);
        }
        if (!ofound)
        {
            return ("", EOptionOutOfRange);
        }
        return (result, EOK);

    }

    public int IsAlias(string name)
    {
        name = PrefixUtil.PrefixedName(name, false);
        if (!World.HasNamedAlias(name))
        {
            return EAliasNotFound;
        }
        return EOK;
    }

    public int SetAliasOption(string name, string option, string value)
    {
        name = PrefixUtil.PrefixedName(name, false);
        var (_, ofound, tfound) = World.SetAliasOption(name, option, value);
        if (!tfound)
        {
            return EAliasNotFound;
        }
        if (!ofound)
        {
            return EOK;
        }
        return EOK;
    }

    public int AddTrigger(string triggerName, string match, string responseText, int flags, int colour, int wildcard, string soundFileName, string scriptName)
    {
        if (match == "")
        {
            return ETriggerCannotBeEmpty;
        }
        var trigger = Trigger.Create();
        trigger.Name = triggerName;
        trigger.Match = match;
        trigger.Send = responseText;
        trigger.Colour = colour;
        trigger.SoundFileName = soundFileName;
        trigger.Script = scriptName;
        trigger.Enabled = (flags & Trigger.TriggerFlagEnabled) != 0;
        trigger.KeepEvaluating = (flags & Trigger.TriggerFlagKeepEvaluating) != 0;
        trigger.IgnoreCase = (flags & Trigger.TriggerFlagIgnoreCase) != 0;
        trigger.OmitFromLog = (flags & Trigger.TriggerFlagOmitFromLog) != 0;
        trigger.Regexp = (flags & Trigger.TriggerFlagRegularExpression) != 0;
        trigger.ExpandVariables = (flags & Trigger.TriggerFlagExpandVariables) != 0;
        trigger.Temporary = (flags & Trigger.TriggerFlagTemporary) != 0;
        trigger.SetByUser(false);
        var ok = World.AddTrigger(trigger, (flags & Trigger.TriggerFlagReplace) != 0);
        if (!ok)
        {
            return ETriggerAlreadyExists;
        }
        return EOK;
    }

    public int AddTriggerEx(string triggerName, string match, string responseText, int flags, int colour, int wildcard, string soundFileName, string scriptName, int sendTo, int sequence)
    {
        if (match == "")
        {
            return ETriggerCannotBeEmpty;
        }
        var trigger = Trigger.Create();
        trigger.Name = triggerName;
        trigger.Match = match;
        trigger.Send = responseText;
        trigger.Colour = colour;
        trigger.SoundFileName = soundFileName;
        trigger.SendTo = sendTo;
        trigger.Sequence = sequence;
        trigger.Script = scriptName;
        trigger.Enabled = (flags & Trigger.TriggerFlagEnabled) != 0;
        trigger.KeepEvaluating = (flags & Trigger.TriggerFlagKeepEvaluating) != 0;
        trigger.IgnoreCase = (flags & Trigger.TriggerFlagIgnoreCase) != 0;
        trigger.OmitFromLog = (flags & Trigger.TriggerFlagOmitFromLog) != 0;
        trigger.Regexp = (flags & Trigger.TriggerFlagRegularExpression) != 0;
        trigger.ExpandVariables = (flags & Trigger.TriggerFlagExpandVariables) != 0;
        trigger.Temporary = (flags & Trigger.TriggerFlagTemporary) != 0;
        trigger.SetByUser(false);
        var ok = World.AddTrigger(trigger, (flags & Trigger.TriggerFlagReplace) != 0);
        if (!ok)
        {
            return ETriggerAlreadyExists;
        }
        return EOK;
    }

    public int DeleteTrigger(string name)
    {
        name = PrefixUtil.PrefixedName(name, false);
        var ok = World.DoDeleteTrigger(name);
        if (!ok)
        {
            return ETriggerNotFound;
        }
        return EOK;
    }
    public int DeleteTriggerGroup(string group)
    {
        return World.DoDeleteTriggerGroup(group, false);
    }
    public int EnableTrigger(string name, bool enabled)
    {
        name = PrefixUtil.PrefixedName(name, false);
        var ok = World.DoEnableTriggerByName(name, enabled);
        if (!ok)
        {
            return ETriggerNotFound;
        }
        return EOK;
    }

    public int EnableTriggerGroup(string group, bool enabled)
    {
        return World.DoEnableTriggerGroup(group, enabled);
    }

    public List<string> GetTriggerList()
    {
        return World.DoListTriggerNames(false);
    }

    public (string, int) GetTriggerOption(string name, string option)
    {
        name = PrefixUtil.PrefixedName(name, false);
        var result = World.GetTriggerOption(name, option);
        if (!result.Found)
        {
            return ("", ETriggerNotFound);
        }
        if (!result.Successed)
        {
            return ("", EOptionOutOfRange);
        }
        return (result.Info, EOK);

    }

    public int IsTrigger(string name)
    {
        name = PrefixUtil.PrefixedName(name, false);
        if (!World.HasNamedTrigger(name))
        {
            return ETriggerNotFound;
        }
        return EOK;
    }

    public int SetTriggerOption(string name, string option, string value)
    {
        name = PrefixUtil.PrefixedName(name, false);
        var (_, ofound, tfound) = World.SetTriggerOption(name, option, value);
        if (!tfound)
        {
            return ETriggerNotFound;
        }
        if (!ofound)
        {
            return EOK;
        }
        return EOK;
    }
    public void StopEvaluatingTriggers()
    {
        World.DoStopEvaluatingTriggers();
    }
    public string? GetTriggerWildcard(string triggername, string wildcard)
    {
        triggername = PrefixUtil.PrefixedName(triggername, false);
        var w = World.DoGetTriggerWildcard(triggername);
        if (w == null)
        {
            return null;
        }
        var result = w.Named.TryGetValue(wildcard, out var r) ? r : null;
        if (result != null)
        {
            return result;
        }
        var index=Int32.TryParse(wildcard, out var idx) ? idx : -1;
        if (index < 0 || index >= w.List.Count)
        {
            return null;
        }
        result = w.List[index];
        return result;
    }
    public int ColourNameToRGB(string v)
    {
        int? color = Colour.NamedColor.TryGetValue(v, out var c) ? c : null;
        if (color == null)
        {
            return -1;
        }
        return color.Value;
    }
    public string MustCleanHomeFileInsidePath(string name)
    {
        var home = World.GetScriptHome();
        name = AuthorizeHelper.CleanPath(home, name);
        if (name == "")
        {
            return name;
        }
        if (!name.StartsWith(home))
        {
            return "";
        }
        return name;
    }
    public bool HasHomeFile(string name)
    {
        var filename = MustCleanHomeFileInsidePath(name);
        if (filename == "")
        {
            throw new Exception($"read {name} not allowed");
        }
        if (!File.Exists(filename))
        {
            return false;
        }
        return true;
    }
    public string ReadHomeFile(string name)
    {
        var filename = MustCleanHomeFileInsidePath(name);
        if (filename == "")
        {
            throw new Exception($"read {name} not allowed");
        }
        var data = File.ReadAllText(filename);
        return data;
    }
    public List<string> ReadHomeLines(string name)
    {
        var data = ReadHomeFile(name);
        return Replacer.Replace(data, lineReplacer).Split("\n").ToList();
    }
    public void WriteHomeFile(string name, byte[] body)
    {
        var filename = MustCleanHomeFileInsidePath(name);
        if (filename == "")
        {
            throw new Exception($"write {name} not allowed");
        }
        File.WriteAllText(filename, Encoding.UTF8.GetString(body));
    }

    public bool MakeHomeFolder(string name)
    {
        var filename = MustCleanHomeFileInsidePath(name);
        if (filename == "")
        {
            throw new Exception($"make folder {name} not allowed");
        }
        Directory.CreateDirectory(filename);
        return true;
    }

    public string MustCleanModFileInsidePath(string name)
    {
        if (!World.GetModEnabled())
        {
            return "";
        }
        var sid = World.GetScriptID();
        if (sid == "")
        {
            return "";
        }
        var modpath = Path.Combine(World.GetModPath(), sid + ".mod");
        name = AuthorizeHelper.CleanPath(modpath, name);
        if (name == "")
        {
            return name;
        }
        if (!name.StartsWith(modpath))
        {
            return "";
        }
        return name;
    }

    public bool HasModFile(string name)
    {
        if (!World.GetModEnabled())
        {
            return false;
        }
        var filename = MustCleanModFileInsidePath(name);
        if (filename == "")
        {
            throw new Exception($"read {name} not allowed");
        }
        if (!File.Exists(filename))
        {
            return false;
        }
        return true;
    }
    public string ReadModFile(string name)
    {
        var filename = MustCleanModFileInsidePath(name);
        if (filename == "")
        {
            throw new Exception($"read {name} not allowed");
        }
        var data = File.ReadAllText(filename);
        return data;
    }
    public List<string> ReadModLines(string name)
    {
        var data = ReadModFile(name);
        return Replacer.Replace(data, lineReplacer).Split("\n").ToList();
    }

    public string MustCleanSharedFileInsidePath(string name)
    {
        var sid = World.GetScriptID();
        if (sid == "")
        {
            return "";
        }
        var modpath = Path.Combine(World.GetSharedPath(), sid);
        name = AuthorizeHelper.CleanPath(modpath, name);
        if (name == "")
        {
            return name;
        }
        if (!name.StartsWith(modpath))
        {
            return "";
        }
        return name;
    }
    public bool MakeSharedFolder(string name)
    {
        var filename = MustCleanSharedFileInsidePath(name);
        if (filename == "")
        {
            throw new Exception($"make folder {name} not allowed");
        }
        Directory.CreateDirectory(filename);
        return true;
    }
    public bool HasSharedFile(string name)
    {
        var filename = MustCleanSharedFileInsidePath(name);
        if (filename == "")
        {
            throw new Exception($"read {name} not allowed");
        }
        if (!File.Exists(filename))
        {
            return false;
        }
        return true;
    }
    public string ReadSharedFile(string name)
    {
        var filename = MustCleanSharedFileInsidePath(name);
        if (filename == "")
        {
            throw new Exception($"read {name} not allowed");
        }
        var data = File.ReadAllText(filename);
        return data;
    }
    public List<string> ReadSharedLines(string name)
    {
        var data = ReadSharedFile(name);
        return Replacer.Replace(data, lineReplacer).Split("\n").ToList();
    }
    public void WriteSharedFile(string name, byte[] body)
    {
        var filename = MustCleanSharedFileInsidePath(name);
        if (filename == "")
        {
            throw new Exception($"write {name} not allowed");
        }
        File.WriteAllBytes(filename, body);
    }

    public bool HasFile(string name)
    {
        var o = World.GetPluginOptions();
        var filename = AuthorizeHelper.CleanInsidePath(o.Location, name);
        if (filename == "")
        {
            throw new Exception($"read {name} not allowed");
        }
        if (!File.Exists(filename))
        {
            return false;
        }
        return true;
    }
    public string ReadFile(string name)
    {
        var o = World.GetPluginOptions();
        var filename = AuthorizeHelper.CleanInsidePath(o.Location, name);
        if (filename == "")
        {
            throw new Exception($"read {name} not allowed");
        }
        var data = File.ReadAllText(filename);
        return data;
    }

    private List<ReplacePair> lineReplacer = new(){
    new("\r\n", "\n"),
    new("\n\r", "\n")
};

    public List<string> ReadLines(string name)
    {
        var data = ReadFile(name);
        return Replacer.Replace(data, lineReplacer).Split("\n").ToList();
    }

    public List<string> SplitN(string text, string sep, int n)
    {
        return text.Split(sep, n).ToList();
    }

    public int UTF8Len(string text)
    {

        return Encoding.UTF8.GetBytes(text).Length;
    }
    public int UTF8Index(string text, string substring)
    {
        var idx = text.IndexOf(substring);
        return idx;
    }
    public string UTF8Sub(string text, int start, int end)
    {


        if (start < 0)
        {
            start = 0;
        }
        if (start >= text.Length)
        {
            return "";
        }
        if (end > text.Length || end <= 0)
        {
            end = text.Length;
        }
        return text.Substring(start, end - start);
    }
    public string? ToUTF8(string code, byte[] text)
    {
        try
        {
            var result = CharsetUtil.ToUtf8(code, text);
            return result;
        }
        catch
        {
            return null;
        }
    }
    public byte[]? FromUTF8(string code, string text)
    {
        try
        {
            return CharsetUtil.FromUtf8(code, text);
        }
        catch
        {
            return null;
        }
    }
    public void Info(string text)
    {
        World.SetStatus(World.GetStatus() + text);
    }
    public void InfoClear()
    {
        World.SetStatus("");
    }

    public string GetAlphaOption(string name)
    {
        switch (name)
        {
            case "name":
                return World.GetName();
            case "id":
                return World.ID;
            case "command_stack_character":
                return World.GetCommandStackCharacter();
            case "script_prefix":
                return World.GetScriptPrefix();
        }
        throw new Exception($"world alpha option {name} not supported");
    }

    public int SetAlphaOption(string name, string value)
    {
        switch (name)
        {
            case "name":
                World.SetName(value);
                break;
            default:
                throw new Exception($"world alpha option {name} not supported");
        }
        return EOK;
    }

    public int GetLinesInBufferCount()
    {
        return World.GetLinesInBufferCount();
    }
    public void DeleteOutput()
    {

    }

    public void DeleteLines(int count)
    {
        World.DoDeleteLines(count);
    }

    public int GetLineCount()
    {
        return World.GetLineCount();
    }

    public string GetRecentLines(int count)
    {
        var recent = World.GetMaxRecent();
        if (count > recent)
        {
            count = recent;
        }
        var lines = World.GetRecentLines(count);
        var result = new List<string>();
        foreach (var v in lines)
        {
            result.Add(v.ToPlainText());
        }
        return string.Join("\n", result);
    }

    public (string, bool) GetLineInfo(int linenumber, int infotype)
    {
        var line = World.GetLine(linenumber);
        if (line == null)
        {
            return ("", false);
        }
        switch (infotype)
        {
            case 1:
                return (line.ToPlainText(), true);
            case 2:
                return (line.ToPlainText().Length.ToString(), true);
            case 3:
                return (MushString.ToStringBool(line.IsNewline()), true);
            case 4:
                return (MushString.ToStringBool(line.Type == Line.LineTypePrint), true);
            case 5:
                return (MushString.ToStringBool(line.Type == Line.LineTypeEcho), true);
            case 6:
                return (MushString.ToStringBool(!line.OmitFromLog), true);
            case 7:
                return (MushString.ToStringBool(false), true);
            case 8:
                return (MushString.ToStringBool(false), true);
            case 9:
                return (line.Time.ToString(), true);
            case 10:
                return (line.ID, true);
            case 11:
                return (line.Words.Count.ToString(), true);
        }
        return ("", false);
    }
    public int BoldColour(int WhichColour)
    {
        //bold colour should equal to normalcolour
        return Colour.GetNormalColour(WhichColour);
    }
    public int NormalColour(int WhichColour)
    {
        return Colour.GetNormalColour(WhichColour);
    }
    public (string, bool) GetStyleInfo(int linenumber, int style, int infotype)
    {
        var line = World.GetLine(linenumber);
        if (line == null)
        {
            return ("", false);
        }
        if (style < 1 || style > line.Words.Count)
        {
            return ("", false);
        }
        var word = line.Words[style - 1];
        switch (infotype)
        {
            case 1:
                return (word.Text, true);
            case 2:
                return (word.Text.Length.ToString(), true);
            case 3:
                var sc = line.GetWordStartColumn(style);
                return (sc.ToString(), true);
            case 8:
                return (MushString.ToStringBool(word.Bold), true);
            case 9:
                return (MushString.ToStringBool(word.Underlined), true);
            case 10:
                return (MushString.ToStringBool(word.Blinking), true);
            case 11:
                return (MushString.ToStringBool(word.Inverse), true);
            case 14:
                return (word.GetColorRGB().ToString(), true);
            case 15:
                return (word.GetBGColorRGB().ToString(), true);
        }
        return ("", false);

    }
    public int WriteLog(string message)
    {
        World.DoLog(message);
        return EOK;
    }
    public int CloseLog()
    {
        return EOK;
    }
    public int FlushLog()
    {
        return EOK;
    }
    public int OpenLog()
    {
        return EOK;
    }
    public string GetGlobalOption(string optionname)
    {
        switch (optionname)
        {
            case "AllTypingToCommandWindow":
            case "AlwaysOnTop":
            case "AppendToLogFiles":
            case "AutoConnectWorlds":
            case "AutoExpandConfig":
            case "FlatToolbars":
            case "AutoLogWorld":
            case "BleedBackground":
            case "ColourGradientConfig":
            case "ConfirmBeforeClosingMXPdebug":
            case "ConfirmBeforeClosingMushclient":
            case "ConfirmBeforeClosingWorld":
            case "ConfirmBeforeSavingVariables":
            case "ConfirmLogFileClose":
            case "EnableSpellCheck":
            case "AllowLoadingDlls":
            case "F1macro":
            case "FixedFontForEditing":
            case "NotepadWordWrap":
            case "NotifyIfCannotConnect":
            case "ErrorNotificationToOutputWindow":
            case "NotifyOnDisconnect":
            case "OpenActivityWindow":
            case "OpenWorldsMaximised":
            case "WindowTabsStyle":
            case "ReconnectOnLinkFailure":
            case "RegexpMatchEmpty":
            case "ShowGridLinesInListViews":
            case "SmoothScrolling":
            case "SmootherScrolling":
            case "DisableKeyboardMenuActivation":
            case "TriggerRemoveCheck":
            case "NotepadBackColour":
            case "NotepadTextColour":
            case "ActivityButtonBarStyle":
            case "AsciiArtLayout":
            case "DefaultInputFontHeight":
            case "DefaultInputFontItalic ":
            case "DefaultInputFontWeight":
            case "DefaultOutputFontHeight":
            case "Icon Placement":
            case "Tray Icon":
            case "ActivityWindowRefreshInterval":
            case "ParenMatchFlags":
            case "PrinterFontSize":
            case "PrinterLeftMargin":
            case "PrinterLinesPerPage":
            case "PrinterTopMargin":
            case "FixedPitchFontSize":
            case "TabInsertsTabInMultiLineDialogs":
            case "AsciiArtFont":
            case "FixedPitchFont":
            case "WordDelimitersDblClick":
                return "0";
            case "TimerInterval":
                return "0";
            case "ActivityWindowRefreshType":
            case "PluginList":
            case "PluginsDirectory":
            case "StateFilesDirectory":
            case "PrinterFont":
            case "TrayIconFileName":
            case "WordDelimiters":
            case "WorldList":
            case "LuaScript":
            case "Locale":
            case "DefaultAliasesFile":
            case "DefaultColoursFile":
            case "DefaultInputFont":
            case "DefaultLogFileDirectory":
            case "DefaultMacrosFile":
            case "DefaultNameGenerationFile":
            case "DefaultOutputFont ":
            case "DefaultTimersFile ":
            case "DefaultTriggersFile":
            case "DefaultWorldFileDirectory":
            case "NotepadQuoteString":
                return "";
        }
        return "";
    }
    public string GetInfo(int infotype)
    {
        switch (infotype)
        {
            case 1:
                return World.GetHost();
            case 2:
                return World.GetName();
            case 8:
                return "";
            case 28:
                return World.GetScriptType();
            case 35:
                return World.GetScriptID();
            case 36:
                return World.GetScriptPrefix();
            case 40:
                return World.ID + ".log";
            case 51:
                return World.ID + ".log";
            case 53:
                return World.GetStatus();
            case 54:
                return World.ID + ".toml";
            case 55:
                return World.ID;
            case 56:
                return "hellclient";
            case 57:
                return "./";
            case 58:
                return "./";
            case 59:
                return "./";
            case 64:
                return "./";
            case 66:
                return "./";
            case 67:
                return "./";
            case 68:
                return "./";

        }
        throw new Exception($"unknown world.GetInfo infotype {infotype}");
    }

    public (string, int) GetTimerInfo(string name, int infotype)
    {
        name = PrefixUtil.PrefixedName(name, false);
        var result = World.GetTimerInfo(name, infotype);
        if (!result.Found)
        {
            return ("", ETimerNotFound);
        }
        if (!result.Successed)
        {
            throw new Exception($"unknown world.GetTimerInfo infotype {infotype}");
        }
        return (result.Info, EOK);

    }

    public (string, int) GetTriggerInfo(string name, int infotype)
    {
        name = PrefixUtil.PrefixedName(name, false);
        var result = World.GetTriggerInfo(name, infotype);
        if (!result.Found)
        {
            return ("", ETriggerNotFound);
        }
        if (!result.Successed)
        {
            throw new Exception($"unknown world.GetTriggerInfo infotype {infotype}");
        }
        return (result.Info, EOK);
    }

    public (string, int) GetAliasInfo(string name, int infotype)
    {
        name = PrefixUtil.PrefixedName(name, false);
        var result = World.GetAliasInfo(name, infotype);
        if (!result.Found)
        {
            return ("", EAliasNotFound);
        }
        if (!result.Successed)
        {
            throw new Exception($"unknown world.GetAliasInfo infotype {infotype}");
        }
        return (result.Info, EOK);
    }
    public void Broadcast(string msg, bool gloabl)
    {
        if (msg == "")
        {
            return;
        }
        var channel = World.GetScriptData()?.Channel ?? "";
        if (channel == "")
        {
            return;
        }
        var bc = Types.Broadcast.CreateBroadcast(channel, msg, gloabl);
        World.EventBus.BroadcastEvent?.Invoke(this, bc);
        if (World.GetShowBroadcast())
        {
            if (gloabl)
            {
                World.DoPrintGlobalBroadcastOut(msg);
            }
            else
            {
                World.DoPrintLocalBroadcastOut(msg);
            }
        }
    }

    public void Notify(string title, string body, string link)
    {
        // notifier.DefaultNotifier.WorldNotify(World.ID, title, body, link);
    }

    public bool CheckPermissions(List<string> p)
    {
        var permissions = World.GetPermissions();

        foreach (var need in p)
        {
            foreach (var own in permissions)
            {
                if (own == need)
                {
                    goto NEED;
                }
            }
            return false;
        NEED:;
        }
        return true;
    }
    public void RequestPermissions(List<string> permissions, string reason, string script)
    {
        World.EventBus.RequestPermissionsEvent?.Invoke(this, new Authorization()
        {
            World = World.ID,
            Items = permissions,
            Reason = reason,
            Script = script
        });
    }
    public bool CheckTrustedDomains(List<string> d)
    {
        var domains = World.GetTrusted().Domains;
        foreach (var need in d)
        {
            foreach (var own in domains)
            {
                if (own == need)
                {
                    goto NEED;
                }
            }
            return false;
        NEED:;
        }
        return true;
    }

    public void RequestTrustDomains(List<string> domains, string reason, string script)
    {
        World.EventBus.RequestTrustDomainsEvent?.Invoke(this, new Authorization()
        {
            World = World.ID,
            Items = domains,
            Reason = reason,
            Script = script
        });
    }

    public string? Encrypt(string data, string key)
    {
        try
        {
            var result = AesUtil.Encrypt(data, key);
            return result;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public string? Decrypt(string data, string key)
    {
        try
        {
            var result = AesUtil.Decrypt(data, key);
            return result;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public string Request(string reqtype, string data)
    {
        var msg = Message.Create(World.ID, reqtype, data);
        World.EventBus.RequestEvent?.Invoke(this, msg);
        if (World.GetShowBroadcast())
        {
            World.DoPrintRequest(msg.Desc());
        }
        return msg.ID;
    }

    public string DumpOutput(int length, int offset)
    {
        if (length < 0)
        {
            length = 0;
        }
        if (offset < 0)
        {
            offset = 0;
        }
        var line = World.GetRecentLines(offset + length);
        if (length > line.Count)
        {
            length = line.Count;
        }
        return WorldJsonContext.Serialize(line.GetRange(0, length));
    }

    public string ConcatOutput(string output1, string output2)
    {
        var list1 = new List<Line>();
        var list2 = new List<Line>();
        // Deserialize output1 into list1
        list1 = JsonSerializer.Deserialize<List<Line>>(output1, WorldJsonContext.Instance.ListLine);
        // Deserialize output2 into list2
        list2 = JsonSerializer.Deserialize<List<Line>>(output2, WorldJsonContext.Instance.ListLine);
        if (list1 == null)
        {
            list1 = new List<Line>();
        }
        if (list2 == null)
        {
            list2 = new List<Line>();
        }
        list1.AddRange(list2);
        return WorldJsonContext.Serialize(list1);
    }
    public string SliceOutput(string output, int start, int end)
    {
        var list = JsonSerializer.Deserialize<List<Line>>(output, WorldJsonContext.Instance.ListLine);
        if (list == null)
        {
            return "";
        }
        if (start < 0)
        {
            start = 0;
        }
        if (start >= list.Count)
        {
            start = list.Count - 1;
        }
        if (end <= 0 || end > list.Count)
        {
            end = list.Count;
        }
        if (end < start)
        {
            end = start - 1;
        }
        return WorldJsonContext.Serialize(list.GetRange(start, end - start));
    }
    public string OutputToText(string output)
    {
        var list = JsonSerializer.Deserialize<List<Line>>(output, WorldJsonContext.Instance.ListLine);
        if (list == null)
        {
            return "";
        }
        var lines = new List<string>(list.Count);
        for (var i = 0; i < list.Count; i++)
        {
            var words = new List<string>(list[i].Words.Count);
            for (var j = 0; j < list[i].Words.Count; j++)
            {
                words.Add(list[i].Words[j].Text);
            }
            lines.Add(string.Join("", words));
        }
        return string.Join("\n", lines);
    }

    public string FormatOutput(string output)
    {
        var list = JsonSerializer.Deserialize<List<Line>>(output, WorldJsonContext.Instance.ListLine);
        if (list == null)
        {
            return "";
        }
        return WorldJsonContext.SerializeLineListIndented(list);

    }

    public string PrintOutput(string text)
    {
        var line = Line.New();
        line.Type = Line.LineTypePrint;
        var word = new Word();
        word.Text = text;
        line.Words.Add(word);
        var list = new List<Line> { line };
        var data = WorldJsonContext.SerializeLineListIndented(list);
        return data;

    }

    public void Simulate(string text)
    {
        var list = text.Split("\n");
        Task.Run(async () =>
        {
            foreach (var t in list)
            {
                var line = Line.New();
                line.Type = Line.LineTypeReal;
                var word = new Word();
                word.Text = t;
                line.Words.Add(word);
                World.EventBus.LineEvent?.Invoke(this, line);
            }
        });
    }

    public void SimulateOutput(string output)
    {
        var list = JsonSerializer.Deserialize<List<Line>>(output, WorldJsonContext.Instance.ListLine);
        if (list == null)
        {
            return;
        }
        Task.Run(async () =>
        {
            foreach (var line in list)
            {
                line.ID = SimpleID.Instance.GenerateID();
                World.EventBus.LineEvent?.Invoke(this, line);
            }
        });
    }

    public string DumpTriggers(bool byUser)
    {
        var t = World.GetTriggersByType(byUser);
        var data = WorldJsonContext.Serialize(t);
        return data;
    }
    public void RestoreTriggers(string data, bool byUser)
    {
        var list = JsonSerializer.Deserialize<List<Trigger>>(data, WorldJsonContext.Instance.ListTrigger);
        if (list == null)
        {
            return;
        }
        foreach (var v in list)
        {
            v.SetByUser(byUser);
        }
        World.AddTriggers(list);
    }
    public string DumpTimers(bool byUser)
    {
        var t = World.GetTimersByType(byUser);
        var data = WorldJsonContext.Serialize(t);
        return data;
    }
    public void RestoreTimers(string data, bool byUser)
    {
        var list = JsonSerializer.Deserialize<List<Timer>>(data, WorldJsonContext.Instance.ListTimer);
        if (list == null)
        {
            return;
        }
        foreach (var v in list)
        {
            v.SetByUser(byUser);
        }
        World.AddTimers(list);
    }
    public string DumpAliases(bool byUser)
    {
        var al = World.GetAliasesByType(byUser);
        var data = WorldJsonContext.Serialize(al);
        return data;
    }
    public void RestoreAliases(string data, bool byUser)
    {
        var list = JsonSerializer.Deserialize<List<Alias>>(data, WorldJsonContext.Instance.ListAlias);
        if (list == null)
        {
            return;
        }
        foreach (var v in list)
        {
            v.SetByUser(byUser);
        }
        World.AddAliases(list);
    }

    public void SetHUDSize(int size)
    {
        World.SetHUDSize(size);
    }

    public string GetHUDContent()
    {
        var content = World.GetHUDContent();
        return JsonSerializer.Serialize<List<Line>>(content, WorldJsonContext.Instance.ListLine);
    }

    public int GetHUDSize()
    {
        return World.GetHUDSize();
    }
    public bool UpdateHUD(int start, string content)
    {
        var lines = JsonSerializer.Deserialize<List<Line>>(content, WorldJsonContext.Instance.ListLine);
        if (lines == null)
        {
            return false;
        }
        return World.UpdateHUDContent(start, lines);
    }
    public string NewLine()
    {
        var line = Line.New();
        line.Type = Line.LineTypeReal;
        var data = WorldJsonContext.Serialize(line);
        return data;
    }
    public string NewWord(string value)
    {
        var word = new Word();
        word.Text = value;
        var data = WorldJsonContext.Serialize(word);
        return data;
    }

    public Mod? GetModInfo()
    {
        var mod = new Mod();
        if (!World.GetModEnabled())
        {
            return mod;

        }
        mod.Enabled = true;
        var modpath = MustCleanModFileInsidePath("");
        if (modpath == "")
        {
            throw new InvalidOperationException("get mod info not allowed");
        }
        if (!Directory.Exists(modpath))
        {
            return mod;
        }
        var files = Directory.GetFiles(modpath);
        mod.Exists = true;
        foreach (var f in files)
        {
            if (Directory.Exists(f))
            {
                mod.FolderList.Add(f);
            }
            else
            {
                mod.FileList.Add(f);
            }
        }
        mod.FolderList.Sort();
        mod.FileList.Sort();
        return mod;
    }

    public void SetPriority(int value)
    {
        World.SetPriority(value);
    }
    public int GetPriority()
    {
        return World.GetPriority();
    }
    public void SetSummary(string content)
    {
        var lines = JsonSerializer.Deserialize<List<Line>>(content, WorldJsonContext.Instance.ListLine);
        if (lines == null)
        {
            return;
        }
        World.SetSummary(lines);
    }
    public string GetSummary()
    {
        var content = World.GetSummary();
        var data = JsonSerializer.Serialize(content, WorldJsonContext.Instance.ListLine);
        return data;
    }

    public bool Save()
    {
        World.EventBus.SaveEvent?.Invoke(this, EventArgs.Empty);
        return true;
    }

    public long Milliseconds()
    {
        return DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }

    public void OmitOutput()
    {
        World.DoOmitOutput();
    }

}