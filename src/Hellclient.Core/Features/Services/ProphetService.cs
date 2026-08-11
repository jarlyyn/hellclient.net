using System.Text;
using System.Text.Json;
using Hellclient.Core.Features.States;
using Hellclient.Core.Types;
using Hellclient.Core.Infras.Components;
using Hellclient.World.Types;
using Hellclient.Core.Types.Forms;
using Hellclient.Core.Helpers;
using Hellclient.World.Utils;

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
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Instance.String)!;
        change(ctx, conn, msg);
    }

    public void OnCmdConnect(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Instance.String)!;
        TitanService.HandleCmdConnect(ctx.TitanContext, msg);

    }

    public void OnCmdDisconnect(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Instance.String)!;
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
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Instance.String)!;
        TitanService.HandleCmdSend(ctx.TitanContext, id, msg);
    }

    public void OnCmdAllLines(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var id = getCurrent(ctx);
        TitanService.HandleCmdAllLines(ctx.TitanContext, id);
    }

    public void OnCmdCreateGame(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var form = JsonSerializer.Deserialize<CreateGameForm>(cmd.Data(), JsonContext.Instance.CreateGameForm)!;
        if (form is null)
        {
            return;
        }
        var errors = FormHelper.ValidateCreateGameForm(form);
        if (errors.Count > 0)
        {
            TitanService.OnCreateFail(ctx.TitanContext, errors);
            return;
        }
        lock (ctx.TitanContext.Worlds)
        {
            var w = TitanService.NewWorld(ctx.TitanContext, form.ID);
            if (w == null)
            {
                return;
            }
            w.SetHost(form.Host);
            w.SetPort(form.Port);
            w.SetCharset(form.Charset);
            TitanService.OnCreateSuccess(ctx.TitanContext, form.ID);
            TitanService.ExecClients(ctx.TitanContext);
            TitanService.SaveWorld(ctx.TitanContext, form.ID);
        }
    }
    public void OnCmdNotOpened(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        TitanService.HandleCmdNotOpened(ctx.TitanContext);
    }
    public async Task OnCmdOpen(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Instance.String)!;

        var ok = await TitanService.HandleCmdOpen(ctx.TitanContext, msg);
        if (ok)
        {
            change(ctx, conn, msg);
            TitanService.ExecClients(ctx.TitanContext);
        }
    }
    public void OnCmdClose(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Instance.String)!;
        TitanService.CloseWorld(ctx.TitanContext, msg);
        change(ctx, conn, "");
        TitanService.ExecClients(ctx.TitanContext);
    }
    public void OnCmdSave(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Instance.String)!;
        TitanService.HandleCmdSave(ctx.TitanContext, msg);
    }
    public void OnCmdSaveScript(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Instance.String)!;
        TitanService.HandleCmdSaveScript(ctx.TitanContext, msg);
    }
    public void OnCmdScriptInfo(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Instance.String)!;
        TitanService.HandleCmdScriptInfo(ctx.TitanContext, msg);
    }
    public void OnCmdCreateScript(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var form = JsonSerializer.Deserialize<CreateScriptForm>(cmd.Data(), JsonContext.Instance.CreateScriptForm)!;
        if (form is null)
        {
            return;
        }
        var errors = FormHelper.ValidateCreateScriptForm(form);
        if (errors.Count > 0)
        {
            TitanService.OnCreateFail(ctx.TitanContext, errors);
            return;
        }
        TitanService.NewScript(ctx.TitanContext, form.ID, form.Type);
        TitanService.OnCreateScriptSuccess(ctx.TitanContext, form.ID);
        TitanService.HandleCmdListScriptInfo(ctx.TitanContext);
    }
    public void OnCmdListScriptinfo(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        TitanService.HandleCmdListScriptInfo(ctx.TitanContext);
    }
    public void OnCmdListStatus(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Instance.String)!;
        TitanService.HandleCmdStatus(ctx.TitanContext, msg);
    }
    public void OnCmdUseScript(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Instance.ListString)!;
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
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Instance.String)!;
        TitanService.HandleCmdReloadScript(ctx.TitanContext, msg);
    }
    public void OnCmdTimers(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Instance.ListString)!;
        TitanService.HandleCmdTimers(ctx.TitanContext, msg[0], msg[1] == "byuser");
    }
    public void OnCmdCreateTimer(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var form = JsonSerializer.Deserialize<CreateTimerForm>(cmd.Data(), JsonContext.Instance.CreateTimerForm)!;
        if (form is null)
        {
            return;
        }
        var errors = FormHelper.ValidateCreateTimerForm(form);
        if (errors.Count > 0)
        {
            TitanService.OnCreateFail(ctx.TitanContext, errors);
            return;
        }
        var timer = FormHelper.CreateTimerFromForm(form);
        var ok = TitanService.DoCreateTimer(ctx.TitanContext, form.World, timer);
        if (!ok)
        {
            TitanService.OnCreateFail(ctx.TitanContext, FormHelper.CreateTimerFailErrors);
            return;
        }
        TitanService.OnCreateTimerSuccess(ctx.TitanContext, form.World, form.Name);
        TitanService.HandleCmdTimers(ctx.TitanContext, form.World, form.ByUser);
        if (form.ByUser)
        {
            TitanService.HandleCmdAutoSaveWorld(ctx.TitanContext, form.World);
        }

    }
    public void OnCmdDeleteTimer(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Instance.ListString)!;
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
                TitanService.HandleCmdAutoSaveWorld(ctx.TitanContext, msg[0]);
            }
        }
    }
    public void OnCmdLoadTimer(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {

        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Instance.ListString)!;
        if (msg.Count < 2)
        {
            return;
        }
        TitanService.HandleCmdLoadTimer(ctx.TitanContext, msg[0], msg[1]);
    }
    public void OnCmdUpdateTimer(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var form = JsonSerializer.Deserialize<UpdateTimerForm>(cmd.Data(), JsonContext.Instance.UpdateTimerForm)!;
        if (form is null)
        {
            return;
        }
        var errors = FormHelper.ValidateUpdateTimerForm(form);
        if (errors.Count > 0)
        {
            TitanService.OnCreateFail(ctx.TitanContext, errors);
            return;
        }
        var model = new World.Types.Timer();
        FormHelper.UpdateTimerFromForm(model, form);
        var result = TitanService.DoUpdateTimer(ctx.TitanContext, form.World, model);
        if (result != MushString.UpdateOK)
        {
            switch (result)
            {
                case MushString.UpdateFailDuplicateName:
                    TitanService.OnCreateFail(ctx.TitanContext, FormHelper.UpdateTimerDuplicateErrors);
                    break;
                case MushString.UpdateFailNotFound:
                    TitanService.OnCreateFail(ctx.TitanContext, FormHelper.UpdateTimerNotFoundErrors);
                    break;
            }
            return;
        }
        TitanService.OnUpdateTimerSuccess(ctx.TitanContext, form.World, form.ID);
        TitanService.HandleCmdTimers(ctx.TitanContext, form.World, form.ByUser);
        if (form.ByUser)
        {
            TitanService.HandleCmdAutoSaveWorld(ctx.TitanContext, form.World);
        }
    }
    public void OnCmdAliases(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Instance.ListString)!;
        if (msg.Count < 2)
        {
            return;
        }
        TitanService.HandleCmdAliases(ctx.TitanContext, msg[0], msg[1] == "byuser");
    }
    public void OnCmdCreateAlias(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var form = JsonSerializer.Deserialize<CreateAliasForm>(cmd.Data(), JsonContext.Instance.CreateAliasForm)!;
        if (form is null)
        {
            return;
        }
        var errors = FormHelper.ValidateCreateAliasForm(form);
        if (errors.Count > 0)
        {
            TitanService.OnCreateFail(ctx.TitanContext, errors);
            return;
        }
        var alias = FormHelper.CreateAliasFromForm(form);
        var ok = TitanService.DoCreateAlias(ctx.TitanContext, form.World, alias);
        if (!ok)
        {
            TitanService.OnCreateFail(ctx.TitanContext, FormHelper.CreateAliasFailErrors);
            return;
        }
        TitanService.OnCreateAliasSuccess(ctx.TitanContext, form.World, form.ID);
        TitanService.HandleCmdAliases(ctx.TitanContext, form.World, form.ByUser);
        if (form.ByUser)
        {
            TitanService.HandleCmdAutoSaveWorld(ctx.TitanContext, form.World);
        }
    }
    public void OnCmdDeleteAlias(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Instance.ListString)!;
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
                TitanService.HandleCmdAutoSaveWorld(ctx.TitanContext, msg[0]);
            }
        }
    }
    public void OnCmdLoadAlias(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Instance.ListString)!;
        if (msg.Count < 2)
        {
            return;
        }
        TitanService.HandleCmdLoadAlias(ctx.TitanContext, msg[0], msg[1]);
    }
    public void OnCmdUpdateAlias(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var form = JsonSerializer.Deserialize<UpdateAliasForm>(cmd.Data(), JsonContext.Instance.UpdateAliasForm)!;
        if (form is null)
        {
            return;
        }
        var errors = FormHelper.ValidateUpdateAliasForm(form);
        if (errors.Count > 0)
        {
            TitanService.OnCreateFail(ctx.TitanContext, errors);
            return;
        }
        var model = new Alias();
        FormHelper.UpdateAliasFromForm(model, form);
        var result = TitanService.DoUpdateAlias(ctx.TitanContext, form.World, model);
        if (result != MushString.UpdateOK)
        {
            switch (result)
            {
                case MushString.UpdateFailDuplicateName:
                    TitanService.OnCreateFail(ctx.TitanContext, FormHelper.UpdateAliasDuplicateErrors);
                    break;
                case MushString.UpdateFailNotFound:
                    TitanService.OnCreateFail(ctx.TitanContext, FormHelper.UpdateAliasNotFoundErrors);
                    break;
            }
            return;
        }
        TitanService.OnUpdateAliasSuccess(ctx.TitanContext, form.World, form.ID);
        TitanService.HandleCmdAliases(ctx.TitanContext, form.World, form.ByUser);
        if (form.ByUser)
        {
            TitanService.HandleCmdAutoSaveWorld(ctx.TitanContext, form.World);
        }
    }
    public void OnCmdTriggers(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Instance.ListString)!;

        TitanService.HandleCmdTriggers(ctx.TitanContext, msg[0], msg[1] == "byuser");
    }
    public void OnCmdCreateTrigger(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var form = JsonSerializer.Deserialize<CreateTriggerForm>(cmd.Data(), JsonContext.Instance.CreateTriggerForm)!;
        if (form is null)
        {
            return;
        }
        var errors = FormHelper.ValidateCreateTriggerForm(form);
        if (errors.Count > 0)
        {
            TitanService.OnCreateFail(ctx.TitanContext, errors);
            return;
        }
        var trigger = FormHelper.CreateTriggerFromForm(form);
        var ok = TitanService.DoCreateTrigger(ctx.TitanContext, form.World, trigger);
        if (!ok)
        {
            TitanService.OnCreateFail(ctx.TitanContext, FormHelper.CreateTriggerFailErrors);
            return;
        }
        TitanService.OnCreateTriggerSuccess(ctx.TitanContext, form.World, form.ID);
        TitanService.HandleCmdTriggers(ctx.TitanContext, form.World, form.ByUser);
        if (form.ByUser)
        {
            TitanService.HandleCmdAutoSaveWorld(ctx.TitanContext, form.World);
        }
    }
    public void OnCmdDeleteTrigger(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Instance.ListString)!;
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
                TitanService.HandleCmdAutoSaveWorld(ctx.TitanContext, msg[0]);
            }
        }
    }
    public void OnCmdLoadTrigger(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Instance.ListString)!;
        if (msg.Count < 2)
        {
            return;
        }
        TitanService.HandleCmdLoadTrigger(ctx.TitanContext, msg[0], msg[1]);
    }
    public void OnCmdUpdateTrigger(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var form = JsonSerializer.Deserialize<UpdateTriggerForm>(cmd.Data(), JsonContext.Instance.UpdateTriggerForm)!;
        if (form is null)
        {
            return;
        }
        var errors = FormHelper.ValidateUpdateTriggerForm(form);
        if (errors.Count > 0)
        {
            TitanService.OnCreateFail(ctx.TitanContext, errors);
            return;
        }
        var model = new Trigger();
        FormHelper.UpdateTriggerFromForm(model, form);
        var result = TitanService.DoUpdateTrigger(ctx.TitanContext, form.World, model);
        if (result != MushString.UpdateOK)
        {
            switch (result)
            {
                case MushString.UpdateFailDuplicateName:
                    TitanService.OnCreateFail(ctx.TitanContext, FormHelper.UpdateTriggerDuplicateErrors);
                    break;
                case MushString.UpdateFailNotFound:
                    TitanService.OnCreateFail(ctx.TitanContext, FormHelper.UpdateTriggerNotFoundErrors);
                    break;
            }
            return;
        }
        TitanService.OnUpdateTriggerSuccess(ctx.TitanContext, form.World, form.ID);
        TitanService.HandleCmdTriggers(ctx.TitanContext, form.World, form.ByUser);
        if (form.ByUser)
        {
            TitanService.HandleCmdAutoSaveWorld(ctx.TitanContext, form.World);
        }
    }
    public void OnCmdParams(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Instance.String)!;
        TitanService.HandleCmdParams(ctx.TitanContext, msg);
    }
    public void OnCmdUpdateParam(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Instance.ListString)!;
        if (msg.Count < 3)
        {
            return;
        }
        TitanService.HandleCmdUpdateParam(ctx.TitanContext, msg[0], msg[1], msg[2]);
    }
    public void OnCmdUpdateParamComment(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Instance.ListString)!;
        if (msg.Count < 3)
        {
            return;
        }
        TitanService.HandleCmdUpdateParamComment(ctx.TitanContext, msg[0], msg[1], msg[2]);
    }

    public void OnCmdUpdateWorldSettings(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var form = JsonSerializer.Deserialize<UpdateGameForm>(cmd.Data(), JsonContext.Instance.UpdateGameForm)!;
        if (form is null)
        {
            return;
        }
        var errors = FormHelper.ValidateUpdateGameForm(form);
        if (errors.Count > 0)
        {
            TitanService.OnCreateFail(ctx.TitanContext, errors);
            return;
        }
        var w = TitanService.World(ctx.TitanContext, form.ID);
        if (w is null)
        {
            return;
        }
        w.Lock.Wait();
        try
        {
            FormHelper.UpdateGameFromForm(w, form);
        }
        finally
        {
            w.Lock.Release();
        }
        TitanService.OnUpdateSuccess(ctx.TitanContext, form.ID);
        TitanService.HandleCmdWorldSettings(ctx.TitanContext, form.ID);
        TitanService.ExecClients(ctx.TitanContext);
    }
    public void OnCmdUpdateScriptSettings(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var form = JsonSerializer.Deserialize<UpdateScriptForm>(cmd.Data(), JsonContext.Instance.UpdateScriptForm)!;
        if (form is null)
        {
            return;
        }
        var errors = FormHelper.ValidateUpdateScriptForm(form);
        if (errors.Count > 0)
        {
            TitanService.OnCreateFail(ctx.TitanContext, errors);
            return;
        }
        var w = TitanService.World(ctx.TitanContext, form.ID);
        if (w is null)
        {
            return;
        }
        w.Lock.Wait();
        try
        {
            var data = w.GetScriptData();
            if (data is null)
            {
                return;
            }
            FormHelper.UpdateScriptFromForm(data, form);
        }
        finally
        {
            w.Lock.Release();
        }
        TitanService.OnUpdateScriptSuccess(ctx.TitanContext, form.ID);
        TitanService.HandleCmdScriptSettings(ctx.TitanContext, form.ID);
        TitanService.ExecClients(ctx.TitanContext);
    }
    public void OnCmdDeleteParam(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Instance.ListString)!;

        if (msg.Count < 2)
        {
            return;
        }
        TitanService.HandleCmdDeleteParam(ctx.TitanContext, msg[0], msg[1]);
    }
    public void OnCmdCallback(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Instance.ListString)!;

        if (msg.Count < 2)
        {
            return;
        }
        var cb = JsonSerializer.Deserialize<Callback>(msg[1], JsonContext.Instance.Callback)!;
        TitanService.HandleCmdCallback(ctx.TitanContext, msg[0], cb);
    }
    public void OnCmdAssist(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Instance.String)!;
        TitanService.HandleCmdAssist(ctx.TitanContext, msg);
    }
    public void OnCmdAbout(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        TitanService.HandleCmdAbout(ctx.TitanContext);
    }

    public void OnCmdWorldSettings(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Instance.String)!;
        TitanService.HandleCmdWorldSettings(ctx.TitanContext, msg);
    }

    public void OnCmdScriptSettings(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Instance.String)!;
        TitanService.HandleCmdScriptSettings(ctx.TitanContext, msg);
    }
    public void OnCmdRequiredParams(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Instance.String)!;
        TitanService.HandleCmdRequiredParams(ctx.TitanContext, msg);
    }
    public void OnCmdUpdateRequiredParams(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<RequiredParamsForm>(cmd.Data(), JsonContext.Instance.RequiredParamsForm)!;
        TitanService.HandleCmdUpdateRequiredParams(ctx.TitanContext, msg.Current, msg.RequiredParams);
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

        var msg = JsonSerializer.Deserialize<Authorization>(cmd.Data(), JsonContext.Instance.Authorization)!;
        TitanService.HandleCmdRequestPermissions(ctx.TitanContext, msg);
    }
    public void OnCmdRequestTrustDomains(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<Authorization>(cmd.Data(), JsonContext.Instance.Authorization)!;

        TitanService.HandleCmdRequestTrustDomains(ctx.TitanContext, msg);
    }
    public void OnCmdAuthorized(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Instance.String)!;
        TitanService.HandleCmdAuthorized(ctx.TitanContext, msg);
    }
    public void OnCmdRevokeAuthorized(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Instance.String)!;
        TitanService.HandleCmdRevokeAuthorized(ctx.TitanContext, msg);
    }
    public void OnCmdMasssend(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var id = getCurrent(ctx);
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Instance.String)!;
        TitanService.HandleCmdMasssend(ctx.TitanContext, id, msg);
    }
    public void OnCmdFindHistory(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var id = getCurrent(ctx);
        var msg = JsonSerializer.Deserialize<int>(cmd.Data(), JsonContext.Instance.Int32)!;
        TitanService.HandleCmdFindHistory(ctx.TitanContext, id, msg);
    }
    public void OnCmdHUDClick(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var id = getCurrent(ctx);
        var msg = JsonSerializer.Deserialize<Click>(cmd.Data(), JsonContext.Instance.Click)!;
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
        var msg = JsonSerializer.Deserialize<List<string>>(cmd.Data(), JsonContext.Instance.ListString)!;
        TitanService.DoSortClients(ctx.TitanContext, msg);
        TitanService.ExecClients(ctx.TitanContext);
    }
    public void OnCmdKeyUp(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var id = getCurrent(ctx);
        var msg = JsonSerializer.Deserialize<string>(cmd.Data(), JsonContext.Instance.String)!;
        TitanService.HandleCmdKeyUp(ctx.TitanContext, id, msg);
    }
    public void OnCmdBatchCommand(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        var msg = JsonSerializer.Deserialize<BatchCommand>(cmd.Data(), JsonContext.Instance.BatchCommand)!;
        TitanService.HandleBatchCommand(ctx.TitanContext, msg);
    }
    public void OnCmdBatchCommandScripts(ProphetContext ctx, IConnection conn, SeparatedCommand cmd)
    {
        TitanService.HandleCmdBatchCommandScripts(ctx.TitanContext);
    }
}