using System.Text;
using System.Text.Json;
using Hellclient.Core.Features.States;
using Hellclient.Core.Types;
using Hellclient.Core.Infras.Components;
using Hellclient.World.Types;
using System.Reflection.Metadata.Ecma335;

namespace Hellclient.Core.Features.Services;

public interface IProphetService
{
    void Enter(ProphetContext ctx, IConnection conn);
    void SendToUser(ProphetContext ctx, byte[] data);
    void Send(ProphetContext ctx, IConnection conn, string msgtype, object data);
    void Change(ProphetContext ctx, string roomid);
    void OnCmdChange(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdConnect(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdDisconnect(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdSend(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdAllLines(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdCreateGame(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdNotOpened(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    Task OnCmdOpen(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdClose(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdSave(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdSaveScript(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdScriptInfo(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdCreateScript(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdListScriptinfo(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdListStatus(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdUseScript(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdReloadScript(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdTimers(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdCreateTimer(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdDeleteTimer(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdLoadTimer(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdUpdateTimer(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdAliases(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdCreateAlias(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdDeleteAlias(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdLoadAlias(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdUpdateAlias(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdTriggers(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdCreateTrigger(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdDeleteTrigger(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdLoadTrigger(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdUpdateTrigger(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdParams(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdUpdateParam(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdUpdateParamComment(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdUpdateWorldSettings(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdUpdateScriptSettings(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdDeleteParam(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdCallback(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdAssist(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdAbout(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdWorldSettings(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdScriptSettings(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdRequiredParams(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdUpdateRequiredParams(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdDefaultServer(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdDefaultCharset(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdRequestPermissions(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdRequestTrustDomains(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdAuthorized(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    void OnCmdRevokeAuthorized(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    public void OnCmdUpdatePassword(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    public void OnCmdFindHistory(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    public void OnCmdMasssend(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    public void OnCmdHUDClick(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    public void OnCmdSortClients(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    public void OnCmdKeyUp(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    public void OnCmdBatchCommand(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    public void OnCmdBatchCommandScripts(ProphetContext ctx, IConnection conn, SeparatedCommand cmd);
    public void OnOpen(ProphetContext ctx, IConnection conn);
    public void OnClose(ProphetContext ctx, IConnection conn);
    public void Publish(ProphetContext ctx, Types.Message message);
    string GetCurrent(ProphetContext ctx);
}
public class ProphetService : IProphetService
{
    public ITitanService TitanService { get; set; } = new TitanService();
    public void Publish(ProphetContext ctx, Types.Message message)
    {
        ctx.Adapter.Exec(message);
    }
    public void OnOpen(ProphetContext ctx, IConnection conn)
    {
        var id = getCurrent(ctx);
        Send(ctx, conn, "current", id);
        if (id != "")
        {

        }
        TitanService.ExecAPIversion(ctx.TitanContext);
        TitanService.ExecClients(ctx.TitanContext);
        TitanService.ExecSwitchStatus(ctx.TitanContext);
        onCurrent(ctx, id);
    }
    public void OnClose(ProphetContext ctx, IConnection conn)
    {
        conn.OnClose = null;
        conn.OnMessage = null;
    }
    private void onCurrent(ProphetContext ctx, string roomid)
    {
        TitanService.Focus(ctx.TitanContext, roomid);
        TitanService.HandleCmdLines(ctx.TitanContext, roomid);
        TitanService.HandleCmdPrompt(ctx.TitanContext, roomid);
        TitanService.HandleCmdStatus(ctx.TitanContext, roomid);
        TitanService.HandleCmdHistory(ctx.TitanContext, roomid);
        TitanService.HandleCmdHUDContent(ctx.TitanContext, roomid);

    }
    private void onLeave(ProphetContext ctx, string roomid)
    {
        TitanService.LoseFocus(ctx.TitanContext, roomid);
    }
    public void Change(ProphetContext ctx, string roomid)
    {
        onLeave(ctx, roomid);
        Interlocked.Exchange(ref ctx.Current!, roomid);
        onCurrent(ctx, roomid);
        TitanService.ExecClients(ctx.TitanContext);
    }
    public void Enter(ProphetContext ctx, IConnection conn)
    {
        conn.OnClose += (sender, e) => OnClose(ctx, conn);
        conn.OnMessage += (sender, e) => ctx.Handlers.Exec(new ConnectionMessage()
        {
            Connection = conn,
            Message = e,
        });
        ctx.Users.Login("user", conn);
        OnOpen(ctx, conn);
    }
    public void SendToUser(ProphetContext ctx, byte[] data)
    {
        ctx.Users.SendByID("user", data);
    }
    public static readonly byte[] SeparatorDefault = new byte[] { 32 };
    public void Send(ProphetContext ctx, IConnection conn, string msgtype, object data)
    {
        var bs = JsonContext.Serialize(data);
        conn.Send(Encoding.UTF8.GetBytes(msgtype).Concat(SeparatorDefault).Concat(bs).ToArray());
    }
    private void change(ProphetContext ctx, IConnection conn, string id)
    {

        Send(ctx, conn, "current", id);
        Change(ctx, id);
    }
    public void OnCmdChange(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Default.String)!;
        change(ctx, conn, msg);
    }

    public void OnCmdConnect(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Default.String)!;
        TitanService.HandleCmdConnect(ctx.TitanContext, msg);

    }

    public void OnCmdDisconnect(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Default.String)!;
        TitanService.HandleCmdDisconnect(ctx.TitanContext, msg);
    }
    public string GetCurrent(ProphetContext ctx)
    {
        return getCurrent(ctx);
    }
    private string getCurrent(ProphetContext ctx)
    {
        return Interlocked.CompareExchange(ref ctx.Current!, null, null);
    }
    public void OnCmdSend(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var id = getCurrent(ctx);
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Default.String)!;
        TitanService.HandleCmdSend(ctx.TitanContext, id, msg);
    }

    public void OnCmdAllLines(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var id = getCurrent(ctx);
        TitanService.HandleCmdAllLines(ctx.TitanContext, id);
    }

    public void OnCmdCreateGame(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        // forms.CreateGame(p.Titan, cmd.Data())
    }
    public void OnCmdNotOpened(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        TitanService.HandleCmdNotOpened(ctx.TitanContext);
    }
    public async Task OnCmdOpen(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Default.String)!;

        var ok = await TitanService.HandleCmdOpen(ctx.TitanContext, msg);
        if (ok)
        {
            change(ctx, conn, msg);
            TitanService.ExecClients(ctx.TitanContext);
        }
    }
    public void OnCmdClose(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Default.String)!;
        TitanService.CloseWorld(ctx.TitanContext, msg);
        change(ctx, conn, "");
        TitanService.ExecClients(ctx.TitanContext);
    }
    public void OnCmdSave(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Default.String)!;
        TitanService.HandleCmdSave(ctx.TitanContext, msg);
    }
    public void OnCmdSaveScript(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Default.String)!;
        TitanService.HandleCmdSaveScript(ctx.TitanContext, msg);
    }
    public void OnCmdScriptInfo(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Default.String)!;
        TitanService.HandleCmdScriptInfo(ctx.TitanContext, msg);
    }
    public void OnCmdCreateScript(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        // forms.CreateScript(p.Titan, cmd.Data())
    }
    public void OnCmdListScriptinfo(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        TitanService.HandleCmdListScriptInfo(ctx.TitanContext);
    }
    public void OnCmdListStatus(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Default.String)!;
        TitanService.HandleCmdStatus(ctx.TitanContext, msg);
    }
    public void OnCmdUseScript(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Default.ListString)!;
        if (msg.Count < 2)
        {
            return;
        }
        TitanService.HandleCmdUseScript(ctx.TitanContext, msg[0], msg[1]);
        TitanService.HandleCmdScriptInfo(ctx.TitanContext, msg[0]);
        TitanService.ExecClients(ctx.TitanContext);
    }

    public void OnCmdReloadScript(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Default.String)!;
        TitanService.HandleCmdReloadScript(ctx.TitanContext, msg);
    }
    public void OnCmdTimers(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Default.ListString)!;
        TitanService.HandleCmdTimers(ctx.TitanContext, msg[0], msg[1] == "byuser");
    }
    public void OnCmdCreateTimer(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        // forms.CreateTimer(p.Titan, cmd.Data())

    }
    public void OnCmdDeleteTimer(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Default.ListString)!;
        if (msg.Count < 2)
        {
            return;
        }
        var itemtype = TitanService.GetTimerType(ctx.TitanContext, msg[0], msg[1]);
        if (itemtype != null)
        {
            TitanService.HandleCmdDeleteTimer(ctx.TitanContext, msg[0], msg[1]);
            TitanService.HandleCmdTimers(ctx.TitanContext, msg[0], itemtype.Value);
            if (itemtype.Value)
            {
                TitanService.AutoSaveWorld(ctx.TitanContext, msg[0]);
            }
        }
    }
    public void OnCmdLoadTimer(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {

        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Default.ListString)!;
        if (msg.Count < 2)
        {
            return;
        }
        TitanService.HandleCmdLoadTimer(ctx.TitanContext, msg[0], msg[1]);
    }
    public void OnCmdUpdateTimer(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        // forms.UpdateTimer(p.Titan, cmd.Data())
    }
    public void OnCmdAliases(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Default.ListString)!;
        if (msg.Count < 2)
        {
            return;
        }
        TitanService.HandleCmdAliases(ctx.TitanContext, msg[0], msg[1] == "byuser");
    }
    public void OnCmdCreateAlias(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        // forms.CreateAlias(p.Titan, cmd.Data())

    }
    public void OnCmdDeleteAlias(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Default.ListString)!;
        if (msg.Count < 2)
        {
            return;
        }
        var itemtype = TitanService.GetAliasType(ctx.TitanContext, msg[0], msg[1]);
        if (itemtype != null)
        {
            TitanService.HandleCmdDeleteAlias(ctx.TitanContext, msg[0], msg[1]);
            TitanService.HandleCmdAliases(ctx.TitanContext, msg[0], itemtype.Value);
            if (itemtype.Value)
            {
                TitanService.AutoSaveWorld(ctx.TitanContext, msg[0]);
            }
        }
    }
    public void OnCmdLoadAlias(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Default.ListString)!;
        if (msg.Count < 2)
        {
            return;
        }
        TitanService.HandleCmdLoadAlias(ctx.TitanContext, msg[0], msg[1]);
    }
    public void OnCmdUpdateAlias(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        // forms.UpdateAlias(p.Titan, cmd.Data())
    }
    public void OnCmdTriggers(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Default.ListString)!;

        TitanService.HandleCmdTriggers(ctx.TitanContext, msg[0], msg[1] == "byuser");
    }
    public void OnCmdCreateTrigger(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        // forms.CreateTrigger(p.Titan, cmd.Data())

    }
    public void OnCmdDeleteTrigger(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Default.ListString)!;
        if (msg.Count < 2)
        {
            return;
        }
        var itemtype = TitanService.GetTriggerType(ctx.TitanContext, msg[0], msg[1]);
        if (itemtype != null)
        {
            TitanService.HandleCmdDeleteTrigger(ctx.TitanContext, msg[0], msg[1]);
            TitanService.HandleCmdTriggers(ctx.TitanContext, msg[0], itemtype.Value);
            if (itemtype.Value)
            {
                TitanService.AutoSaveWorld(ctx.TitanContext, msg[0]);
            }
        }
    }
    public void OnCmdLoadTrigger(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Default.ListString)!;
        if (msg.Count < 2)
        {
            return;
        }
        TitanService.HandleCmdLoadTrigger(ctx.TitanContext, msg[0], msg[1]);
    }
    public void OnCmdUpdateTrigger(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        // forms.UpdateTrigger(p.Titan, cmd.Data())
    }
    public void OnCmdParams(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Default.String)!;
        TitanService.HandleCmdParams(ctx.TitanContext, msg);
    }
    public void OnCmdUpdateParam(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Default.ListString)!;
        if (msg.Count < 3)
        {
            return;
        }
        TitanService.HandleCmdUpdateParam(ctx.TitanContext, msg[0], msg[1], msg[2]);
    }
    public void OnCmdUpdateParamComment(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Default.ListString)!;
        if (msg.Count < 3)
        {
            return;
        }
        TitanService.HandleCmdUpdateParamComment(ctx.TitanContext, msg[0], msg[1], msg[2]);
    }

    public void OnCmdUpdateWorldSettings(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        // forms.UpdateGame(p.Titan, cmd.Data())
    }
    public void OnCmdUpdateScriptSettings(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        // forms.UpdateScript(p.Titan, cmd.Data())
    }
    public void OnCmdDeleteParam(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Default.ListString)!;

        if (msg.Count < 2)
        {
            return;
        }
        TitanService.HandleCmdDeleteParam(ctx.TitanContext, msg[0], msg[1]);
    }
    public void OnCmdCallback(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Default.ListString)!;

        if (msg.Count < 2)
        {
            return;
        }
        var cb = JsonSerializer.Deserialize<Callback>(msg[1], JsonContext.Default.Callback)!;
        TitanService.HandleCmdCallback(ctx.TitanContext, msg[0], cb);
    }
    public void OnCmdAssist(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Default.String)!;
        TitanService.HandleCmdAssist(ctx.TitanContext, msg);
    }
    public void OnCmdAbout(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        TitanService.HandleCmdAbout(ctx.TitanContext);
    }

    public void OnCmdWorldSettings(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Default.String)!;
        TitanService.HandleCmdWorldSettings(ctx.TitanContext, msg);
    }

    public void OnCmdScriptSettings(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Default.String)!;
        TitanService.HandleCmdScriptSettings(ctx.TitanContext, msg);
    }
    public void OnCmdRequiredParams(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Default.String)!;
        TitanService.HandleCmdRequiredParams(ctx.TitanContext, msg);
    }
    public void OnCmdUpdateRequiredParams(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        // var msg = forms.RequiredParamsForm{ }
        // if json.Unmarshal(cmd.Data(), &msg) != nil {



        // }
        // p.Titan.HandleCmdUpdateRequiredParams(msg.Current, msg.RequiredParams)
    }
    public void OnCmdDefaultServer(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        TitanService.HandleCmdDefaultServer(ctx.TitanContext);
    }
    public void OnCmdDefaultCharset(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        TitanService.HandleCmdDefaultCharset(ctx.TitanContext);
    }
    public void OnCmdRequestPermissions(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {

        var msg = JsonSerializer.Deserialize<Authorization>(cmd.Data(), JsonContext.Default.Authorization)!;
        TitanService.HandleCmdRequestPermissions(ctx.TitanContext, msg);
    }
    public void OnCmdRequestTrustDomains(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<Authorization>(cmd.Data(), JsonContext.Default.Authorization)!;

        TitanService.HandleCmdRequestTrustDomains(ctx.TitanContext, msg);
    }
    public void OnCmdAuthorized(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Default.String)!;
        TitanService.HandleCmdAuthorized(ctx.TitanContext, msg);
    }
    public void OnCmdRevokeAuthorized(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Default.String)!;
        TitanService.HandleCmdRevokeAuthorized(ctx.TitanContext, msg);
    }
    public void OnCmdMasssend(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var id = getCurrent(ctx);
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Default.String)!;
        TitanService.HandleCmdMasssend(ctx.TitanContext, id, msg);
    }
    public void OnCmdFindHistory(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var id = getCurrent(ctx);
        var msg = JsonSerializer.Deserialize<int>(cmd.Data(), JsonContext.Default.Int32)!;
        TitanService.HandleCmdFindHistory(ctx.TitanContext, id, msg);
    }
    public void OnCmdHUDClick(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var id = getCurrent(ctx);
        var msg = JsonSerializer.Deserialize<Click>(cmd.Data(), JsonContext.Default.Click)!;
        TitanService.HandleCmdHUDClick(ctx.TitanContext, id, msg);
    }

    public void OnCmdUpdatePassword(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        // if forms.UpdatePassword(p.Titan, cmd.Data()) {
        //     conn.Close()
        // }
    }

    public void OnCmdSortClients(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Default.ListString)!;
        TitanService.DoSortClients(ctx.TitanContext, msg);
        TitanService.ExecClients(ctx.TitanContext);
    }
    public void OnCmdKeyUp(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var id = getCurrent(ctx);
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Default.String)!;
        TitanService.HandleCmdKeyUp(ctx.TitanContext, id, msg);
    }
    public void OnCmdBatchCommand(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<BatchCommand>(cmd.Data(), JsonContext.Default.BatchCommand)!;
        TitanService.HandleBatchCommand(ctx.TitanContext, msg);
    }
    public void OnCmdBatchCommandScripts(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        TitanService.HandleCmdBatchCommandScripts(ctx.TitanContext);
    }

}