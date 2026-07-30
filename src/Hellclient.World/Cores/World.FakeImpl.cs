using Hellclient.World.States;
using Hellclient.World.Types;
using Hellclient.World.Cores;
using Timer = Hellclient.World.Types.Timer;


namespace Hellclient.World.Cores;

public partial class World
{
    public void RequestPermissions(Authorization authorization) { }

    public string GetScriptType() => string.Empty;
    public string GetScriptPath() => string.Empty;
    public string GetModPath() => string.Empty;
    public string GetCorePath() => string.Empty;
    public string GetScriptModPath() => string.Empty;
    public string GetLogsPath() => string.Empty;
    public string GetScriptHome() => string.Empty;
    public void DoLog(string message) { }
    public void RequestTrustDomains(Authorization authorization) { }
    public byte[] DoEncode() => (Array.Empty<byte>());
    public void DoDecode(byte[] data) {}
    public void HandleConnReceive(byte[] msg) { }
    public void HandleConnError(Exception err) { }
    public void HandleConnPrompt(byte[] msg) { }
    public void HandleConverterError(Exception err) { }
    public void HandleCmdError(Exception err) { }
    public void HandleTriggerError(Exception err) { }
    public void HandleScriptError(Exception err) { }


}