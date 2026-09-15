

using Hellclient.PythonEngine.Features.States;
using Python.Runtime;

namespace Hellclient.PythonEngine.Features.Services;

public partial class PythonScriptService
{
    private void AppendToWorld(PyModule scope, PyDict world, string name, PyObject call)
    {
        scope.Set(name, call);
        world[name.ToLower()] = call;
        if (name.ToLower() != name)
        {
            world[name] = call;
        }
    }

    private void initPyAPI(PythonEngineContext context)
    {
        var local = context.Scope;
        var a = context.PyAPI;
        using var world = new PyDict();
        if (world == null)
        {
            return;
        }
#pragma warning disable CS8974 // 将方法组转换为非委托类型
        AppendToWorld(local, world, "print", PyObject.FromManagedObject(a.Print));
        AppendToWorld(local, world, "Note", PyObject.FromManagedObject(a.Note));
        AppendToWorld(local, world, "SendImmediate", PyObject.FromManagedObject(a.SendImmediate));
        AppendToWorld(local, world, "Send", PyObject.FromManagedObject(a.Send));
        AppendToWorld(local, world, "SendNoEcho", PyObject.FromManagedObject(a.SendNoEcho));
        AppendToWorld(local, world, "GetVariable", PyObject.FromManagedObject(a.GetVariable));
        AppendToWorld(local, world, "SetVariable", PyObject.FromManagedObject(a.SetVariable));
        AppendToWorld(local, world, "DeleteVariable", PyObject.FromManagedObject(a.DeleteVariable));
        AppendToWorld(local, world, "GetVariableList", PyObject.FromManagedObject(a.GetVariableList));
        AppendToWorld(local, world, "GetVariableComment", PyObject.FromManagedObject(a.GetVariableComment));
        AppendToWorld(local, world, "SetVariableComment", PyObject.FromManagedObject(a.SetVariableComment));
        AppendToWorld(local, world, "Version", PyObject.FromManagedObject(a.Version));
        AppendToWorld(local, world, "Hash", PyObject.FromManagedObject(a.Hash));
        AppendToWorld(local, world, "Base64Encode", PyObject.FromManagedObject(a.Base64Encode));
        AppendToWorld(local, world, "Base64Decode", PyObject.FromManagedObject(a.Base64Decode));
        AppendToWorld(local, world, "Connect", PyObject.FromManagedObject(a.Connect));
        AppendToWorld(local, world, "IsConnected", PyObject.FromManagedObject(a.IsConnected));
        AppendToWorld(local, world, "Disconnect", PyObject.FromManagedObject(a.Disconnect));
        AppendToWorld(local, world, "GetWorldById", PyObject.FromManagedObject(a.GetWorldById));
        AppendToWorld(local, world, "GetWorld", PyObject.FromManagedObject(a.GetWorld));
        AppendToWorld(local, world, "GetWorldID", PyObject.FromManagedObject(a.GetWorldID));
        AppendToWorld(local, world, "GetWorldIdList", PyObject.FromManagedObject(a.GetWorldIdList));
        AppendToWorld(local, world, "GetWorldList", PyObject.FromManagedObject(a.GetWorldList));
        AppendToWorld(local, world, "WorldName", PyObject.FromManagedObject(a.WorldName));
        AppendToWorld(local, world, "WorldAddress", PyObject.FromManagedObject(a.WorldAddress));
        AppendToWorld(local, world, "WorldPort", PyObject.FromManagedObject(a.WorldPort));
        AppendToWorld(local, world, "WorldProxy", PyObject.FromManagedObject(a.WorldProxy));
        AppendToWorld(local, world, "Trim", PyObject.FromManagedObject(a.Trim));
        AppendToWorld(local, world, "GetUniqueNumber", PyObject.FromManagedObject(a.GetUniqueNumber));
        AppendToWorld(local, world, "GetUniqueID", PyObject.FromManagedObject(a.GetUniqueID));
        AppendToWorld(local, world, "CreateGUID", PyObject.FromManagedObject(a.CreateGUID));
        AppendToWorld(local, world, "FlashIcon", PyObject.FromManagedObject(a.FlashIcon));
        AppendToWorld(local, world, "SetStatus", PyObject.FromManagedObject(a.SetStatus));
        AppendToWorld(local, world, "Execute", PyObject.FromManagedObject(a.Execute));
        AppendToWorld(local, world, "DeleteCommandHistory", PyObject.FromManagedObject(a.DeleteCommandHistory));
        AppendToWorld(local, world, "DiscardQueue", PyObject.FromManagedObject(a.DiscardQueue));
        AppendToWorld(local, world, "LockQueue", PyObject.FromManagedObject(a.LockQueue));
        AppendToWorld(local, world, "GetQueue", PyObject.FromManagedObject(a.GetQueue));
        AppendToWorld(local, world, "Queue", PyObject.FromManagedObject(a.Queue));
        AppendToWorld(local, world, "DoAfter", PyObject.FromManagedObject(a.DoAfter));
        AppendToWorld(local, world, "DoAfterNote", PyObject.FromManagedObject(a.DoAfterNote));
        AppendToWorld(local, world, "DoAfterSpeedWalk", PyObject.FromManagedObject(a.DoAfterSpeedWalk));
        AppendToWorld(local, world, "DoAfterSpecial", PyObject.FromManagedObject(a.DoAfterSpecial));
        AppendToWorld(local, world, "DeleteGroup", PyObject.FromManagedObject(a.DeleteGroup));
        AppendToWorld(local, world, "AddTimer", PyObject.FromManagedObject(a.AddTimer));
        AppendToWorld(local, world, "DeleteTimer", PyObject.FromManagedObject(a.DeleteTimer));
        AppendToWorld(local, world, "DeleteTemporaryTimers", PyObject.FromManagedObject(a.DeleteTemporaryTimers));
        AppendToWorld(local, world, "DeleteTimerGroup", PyObject.FromManagedObject(a.DeleteTimerGroup));
        AppendToWorld(local, world, "EnableTimer", PyObject.FromManagedObject(a.EnableTimer));
        AppendToWorld(local, world, "EnableTimerGroup", PyObject.FromManagedObject(a.EnableTimerGroup));
        AppendToWorld(local, world, "GetTimerList", PyObject.FromManagedObject(a.GetTimerList));
        AppendToWorld(local, world, "IsTimer", PyObject.FromManagedObject(a.IsTimer));
        AppendToWorld(local, world, "ResetTimer", PyObject.FromManagedObject(a.ResetTimer));
        AppendToWorld(local, world, "ResetTimers", PyObject.FromManagedObject(a.ResetTimers));
        AppendToWorld(local, world, "GetTimerOption", PyObject.FromManagedObject(a.GetTimerOption));
        AppendToWorld(local, world, "SetTimerOption", PyObject.FromManagedObject(a.SetTimerOption));
        AppendToWorld(local, world, "AddAlias", PyObject.FromManagedObject(a.AddAlias));
        AppendToWorld(local, world, "DeleteAlias", PyObject.FromManagedObject(a.DeleteAlias));
        AppendToWorld(local, world, "DeleteTemporaryAliases", PyObject.FromManagedObject(a.DeleteTemporaryAliases));
        AppendToWorld(local, world, "DeleteAliasGroup", PyObject.FromManagedObject(a.DeleteAliasGroup));
        AppendToWorld(local, world, "EnableAlias", PyObject.FromManagedObject(a.EnableAlias));
        AppendToWorld(local, world, "EnableAliasGroup", PyObject.FromManagedObject(a.EnableAliasGroup));
        AppendToWorld(local, world, "GetAliasList", PyObject.FromManagedObject(a.GetAliasList));
        AppendToWorld(local, world, "IsAlias", PyObject.FromManagedObject(a.IsAlias));
        AppendToWorld(local, world, "GetAliasOption", PyObject.FromManagedObject(a.GetAliasOption));
        AppendToWorld(local, world, "SetAliasOption", PyObject.FromManagedObject(a.SetAliasOption));
        AppendToWorld(local, world, "AddTrigger", PyObject.FromManagedObject(a.AddTrigger));
        AppendToWorld(local, world, "AddTriggerEx", PyObject.FromManagedObject(a.AddTriggerEx));
        AppendToWorld(local, world, "DeleteTrigger", PyObject.FromManagedObject(a.DeleteTrigger));
        AppendToWorld(local, world, "DeleteTemporaryTriggers", PyObject.FromManagedObject(a.DeleteTemporaryTriggers));
        AppendToWorld(local, world, "DeleteTriggerGroup", PyObject.FromManagedObject(a.DeleteTriggerGroup));
        AppendToWorld(local, world, "EnableTrigger", PyObject.FromManagedObject(a.EnableTrigger));
        AppendToWorld(local, world, "EnableTriggerGroup", PyObject.FromManagedObject(a.EnableTriggerGroup));
        AppendToWorld(local, world, "GetTriggerList", PyObject.FromManagedObject(a.GetTriggerList));
        AppendToWorld(local, world, "IsTrigger", PyObject.FromManagedObject(a.IsTrigger));
        AppendToWorld(local, world, "GetTriggerOption", PyObject.FromManagedObject(a.GetTriggerOption));
        AppendToWorld(local, world, "SetTriggerOption", PyObject.FromManagedObject(a.SetTriggerOption));
        AppendToWorld(local, world, "StopEvaluatingTriggers", PyObject.FromManagedObject(a.StopEvaluatingTriggers));
        AppendToWorld(local, world, "GetTriggerWildcard", PyObject.FromManagedObject(a.GetTriggerWildcard));
        AppendToWorld(local, world, "ColourNameToRGB", PyObject.FromManagedObject(a.ColourNameToRGB));
        AppendToWorld(local, world, "SetSpeedWalkDelay", PyObject.FromManagedObject(a.SetSpeedWalkDelay));
        AppendToWorld(local, world, "GetSpeedWalkDelay", PyObject.FromManagedObject(a.GetSpeedWalkDelay));
        AppendToWorld(local, world, "HasFile", PyObject.FromManagedObject(a.NewHasFileAPI));
        AppendToWorld(local, world, "ReadFile", PyObject.FromManagedObject(a.NewReadFileAPI));
        AppendToWorld(local, world, "ReadLines", PyObject.FromManagedObject(a.NewReadLinesAPI));
        AppendToWorld(local, world, "HasModFile", PyObject.FromManagedObject(a.NewHasModFileAPI));
        AppendToWorld(local, world, "ReadModFile", PyObject.FromManagedObject(a.NewReadModFileAPI));
        AppendToWorld(local, world, "ReadModLines", PyObject.FromManagedObject(a.NewReadModLinesAPI));
        AppendToWorld(local, world, "GetModInfo", PyObject.FromManagedObject(a.NewGetModInfoAPI));
        AppendToWorld(local, world, "MakeHomeFolder", PyObject.FromManagedObject(a.NewMakeHomeFolderAPI));
        AppendToWorld(local, world, "HasHomeFile", PyObject.FromManagedObject(a.NewHasHomeFileAPI));
        AppendToWorld(local, world, "ReadHomeFile", PyObject.FromManagedObject(a.NewReadHomeFileAPI));
        AppendToWorld(local, world, "ReadHomeLines", PyObject.FromManagedObject(a.NewReadHomeLinesAPI));
        AppendToWorld(local, world, "WriteHomeFile", PyObject.FromManagedObject(a.NewWriteHomeFileAPI));
        AppendToWorld(local, world, "MakeSharedFolder", PyObject.FromManagedObject(a.NewMakeSharedFolderAPI));
        AppendToWorld(local, world, "HasSharedFile", PyObject.FromManagedObject(a.NewHasSharedFileAPI));
        AppendToWorld(local, world, "ReadSharedFile", PyObject.FromManagedObject(a.NewReadSharedFileAPI));
        AppendToWorld(local, world, "ReadSharedLines", PyObject.FromManagedObject(a.NewReadSharedLinesAPI));
        AppendToWorld(local, world, "WriteSharedFile", PyObject.FromManagedObject(a.NewWriteSharedFileAPI));
        AppendToWorld(local, world, "SplitN", PyObject.FromManagedObject(a.SplitNfunc));
        AppendToWorld(local, world, "UTF8Len", PyObject.FromManagedObject(a.UTF8Len));
        AppendToWorld(local, world, "UTF8Index", PyObject.FromManagedObject(a.UTF8Index));
        AppendToWorld(local, world, "UTF8Sub", PyObject.FromManagedObject(a.UTF8Sub));
        AppendToWorld(local, world, "ToUTF8", PyObject.FromManagedObject(a.ToUTF8));
        AppendToWorld(local, world, "FromUTF8", PyObject.FromManagedObject(a.FromUTF8));
        AppendToWorld(local, world, "Info", PyObject.FromManagedObject(a.Info));
        AppendToWorld(local, world, "InfoClear", PyObject.FromManagedObject(a.InfoClear));
        AppendToWorld(local, world, "GetAlphaOption", PyObject.FromManagedObject(a.GetAlphaOption));
        AppendToWorld(local, world, "SetAlphaOption", PyObject.FromManagedObject(a.SetAlphaOption));
        AppendToWorld(local, world, "GetLinesInBufferCount", PyObject.FromManagedObject(a.GetLinesInBufferCount));
        AppendToWorld(local, world, "DeleteOutput", PyObject.FromManagedObject(a.DeleteOutput));
        AppendToWorld(local, world, "DeleteLines", PyObject.FromManagedObject(a.DeleteLines));
        AppendToWorld(local, world, "GetLineCount", PyObject.FromManagedObject(a.GetLineCount));
        AppendToWorld(local, world, "GetRecentLines", PyObject.FromManagedObject(a.GetRecentLines));
        AppendToWorld(local, world, "GetLineInfo", PyObject.FromManagedObject(a.GetLineInfo));
        AppendToWorld(local, world, "BoldColour", PyObject.FromManagedObject(a.BoldColour));
        AppendToWorld(local, world, "NormalColour", PyObject.FromManagedObject(a.NormalColour));
        AppendToWorld(local, world, "GetStyleInfo", PyObject.FromManagedObject(a.GetStyleInfo));
        AppendToWorld(local, world, "GetInfo", PyObject.FromManagedObject(a.GetInfo));
        AppendToWorld(local, world, "GetTimerInfo", PyObject.FromManagedObject(a.GetTimerInfo));
        AppendToWorld(local, world, "GetTriggerInfo", PyObject.FromManagedObject(a.GetTriggerInfo));
        AppendToWorld(local, world, "GetAliasInfo", PyObject.FromManagedObject(a.GetAliasInfo));
        AppendToWorld(local, world, "WriteLog", PyObject.FromManagedObject(a.WriteLog));
        AppendToWorld(local, world, "CloseLog", PyObject.FromManagedObject(a.CloseLog));
        AppendToWorld(local, world, "OpenLog", PyObject.FromManagedObject(a.OpenLog));
        AppendToWorld(local, world, "FlushLog", PyObject.FromManagedObject(a.FlushLog));
        AppendToWorld(local, world, "Broadcast", PyObject.FromManagedObject(a.Broadcast));
        AppendToWorld(local, world, "Notify", PyObject.FromManagedObject(a.Notify));
        AppendToWorld(local, world, "Request", PyObject.FromManagedObject(a.Request));
        AppendToWorld(local, world, "GetGlobalOption", PyObject.FromManagedObject(a.GetGlobalOption));
        AppendToWorld(local, world, "CheckPermissions", PyObject.FromManagedObject(a.CheckPermissions));
        AppendToWorld(local, world, "RequestPermissions", PyObject.FromManagedObject(a.RequestPermissions));
        AppendToWorld(local, world, "CheckTrustedDomains", PyObject.FromManagedObject(a.CheckTrustedDomains));
        AppendToWorld(local, world, "RequestTrustDomains", PyObject.FromManagedObject(a.RequestTrustDomains));
        AppendToWorld(local, world, "Encrypt", PyObject.FromManagedObject(a.Encrypt));
        AppendToWorld(local, world, "Decrypt", PyObject.FromManagedObject(a.Decrypt));
        AppendToWorld(local, world, "DumpOutput", PyObject.FromManagedObject(a.DumpOutput));
        AppendToWorld(local, world, "ConcatOutput", PyObject.FromManagedObject(a.ConcatOutput));
        AppendToWorld(local, world, "SliceOutput", PyObject.FromManagedObject(a.SliceOutput));
        AppendToWorld(local, world, "OutputToText", PyObject.FromManagedObject(a.OutputToText));
        AppendToWorld(local, world, "FormatOutput", PyObject.FromManagedObject(a.FormatOutput));
        AppendToWorld(local, world, "PrintOutput", PyObject.FromManagedObject(a.PrintOutput));
        AppendToWorld(local, world, "Simulate", PyObject.FromManagedObject(a.Simulate));
        AppendToWorld(local, world, "SimulateOutput", PyObject.FromManagedObject(a.SimulateOutput));
        AppendToWorld(local, world, "DumpTriggers", PyObject.FromManagedObject(a.DumpTriggers));
        AppendToWorld(local, world, "RestoreTriggers", PyObject.FromManagedObject(a.RestoreTriggers));
        AppendToWorld(local, world, "DumpTimers", PyObject.FromManagedObject(a.DumpTimers));
        AppendToWorld(local, world, "RestoreTimers", PyObject.FromManagedObject(a.RestoreTimers));
        AppendToWorld(local, world, "DumpAliases", PyObject.FromManagedObject(a.DumpAliases));
        AppendToWorld(local, world, "RestoreAliases", PyObject.FromManagedObject(a.RestoreAliases));
        AppendToWorld(local, world, "SetHUDSize", PyObject.FromManagedObject(a.SetHUDSize));
        AppendToWorld(local, world, "GetHUDContent", PyObject.FromManagedObject(a.GetHUDContent));
        AppendToWorld(local, world, "GetHUDSize", PyObject.FromManagedObject(a.GetHUDSize));
        AppendToWorld(local, world, "UpdateHUD", PyObject.FromManagedObject(a.UpdateHUD));
        AppendToWorld(local, world, "NewLine", PyObject.FromManagedObject(a.NewLine));
        AppendToWorld(local, world, "NewWord", PyObject.FromManagedObject(a.NewWord));
        AppendToWorld(local, world, "SetPriority", PyObject.FromManagedObject(a.SetPriority));
        AppendToWorld(local, world, "GetPriority", PyObject.FromManagedObject(a.GetPriority));
        AppendToWorld(local, world, "SetSummary", PyObject.FromManagedObject(a.SetSummary));
        AppendToWorld(local, world, "GetSummary", PyObject.FromManagedObject(a.GetSummary));
        AppendToWorld(local, world, "Save", PyObject.FromManagedObject(a.Save));
        AppendToWorld(local, world, "Milliseconds", PyObject.FromManagedObject(a.Milliseconds));
        AppendToWorld(local, world, "OmitOutput", PyObject.FromManagedObject(a.OmitOutput));
        AppendToWorld(local, world, "PrintSystem", PyObject.FromManagedObject(a.PrintSystem));
        AppendToWorld(local, world, "AddAnsi", PyObject.FromManagedObject(a.AddAnsi));
        AppendToWorld(local, world, "LastAnsi", PyObject.FromManagedObject(a.LastAnsi));


#pragma warning restore CS8974 // 将方法组转换为非委托类型
        local.Set("world", world);
    }
}