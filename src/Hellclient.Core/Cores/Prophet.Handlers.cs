using Hellclient.Core.Types;

namespace Hellclient.Core.Cores;

public partial class Prophet
{

    private void initHandlers()
    {
        //切换游戏指令
        Context.Handlers.RegisterHandler("change", (conn, cmd) => ProphetService.OnCmdChange(Context, conn, cmd));
        //连线指令
        Context.Handlers.RegisterHandler("connect", (conn, cmd) => ProphetService.OnCmdConnect(Context, conn, cmd));
        //断线指令
        Context.Handlers.RegisterHandler("disconnect", (conn, cmd) => ProphetService.OnCmdDisconnect(Context, conn, cmd));
        //发送用户输入指令
        Context.Handlers.RegisterHandler("send", (conn, cmd) => ProphetService.OnCmdSend(Context, conn, cmd));
        //历史信息指令
        Context.Handlers.RegisterHandler("allLines", (conn, cmd) => ProphetService.OnCmdAllLines(Context, conn, cmd));
        //创建游戏指令
        Context.Handlers.RegisterHandler("createGame", (conn, cmd) => ProphetService.OnCmdCreateGame(Context, conn, cmd));
        //未打开游戏清单指令，由于显示打开界面
        Context.Handlers.RegisterHandler("notopened", (conn, cmd) => ProphetService.OnCmdNotOpened(Context, conn, cmd));
        //打开游戏指令
        Context.Handlers.RegisterHandler("open", async (conn, cmd) => await ProphetService.OnCmdOpen(Context, conn, cmd));
        //关闭游戏指令
        Context.Handlers.RegisterHandler("close", (conn, cmd) => ProphetService.OnCmdClose(Context, conn, cmd));
        //保存游戏指令
        Context.Handlers.RegisterHandler("save", (conn, cmd) => ProphetService.OnCmdSave(Context, conn, cmd));
        //请求脚本信息指令
        Context.Handlers.RegisterHandler("scriptinfo", (conn, cmd) => ProphetService.OnCmdScriptInfo(Context, conn, cmd));
        //创建脚本指令
        Context.Handlers.RegisterHandler("createScript", (conn, cmd) => ProphetService.OnCmdCreateScript(Context, conn, cmd));
        //列出全部脚本指令
        Context.Handlers.RegisterHandler("listScriptinfo", (conn, cmd) => ProphetService.OnCmdListScriptinfo(Context, conn, cmd));
        //使用脚本指令
        Context.Handlers.RegisterHandler("usescript", (conn, cmd) => ProphetService.OnCmdUseScript(Context, conn, cmd));
        //保存脚本指令
        Context.Handlers.RegisterHandler("savescript", (conn, cmd) => ProphetService.OnCmdSaveScript(Context, conn, cmd));
        //重新加载脚本指令
        Context.Handlers.RegisterHandler("reloadScript", (conn, cmd) => ProphetService.OnCmdReloadScript(Context, conn, cmd));
        //获取状态行内容指令
        Context.Handlers.RegisterHandler("status", (conn, cmd) => ProphetService.OnCmdListStatus(Context, conn, cmd));
        //获取计时器清单指令
        Context.Handlers.RegisterHandler("timers", (conn, cmd) => ProphetService.OnCmdTimers(Context, conn, cmd));
        //创建计时器指令
        Context.Handlers.RegisterHandler("createTimer", (conn, cmd) => ProphetService.OnCmdCreateTimer(Context, conn, cmd));
        //删除计时器指令
        Context.Handlers.RegisterHandler("deleteTimer", (conn, cmd) => ProphetService.OnCmdDeleteTimer(Context, conn, cmd));
        //获取单个计时器指令
        Context.Handlers.RegisterHandler("loadTimer", (conn, cmd) => ProphetService.OnCmdLoadTimer(Context, conn, cmd));
        //更新计时器指令
        Context.Handlers.RegisterHandler("updateTimer", (conn, cmd) => ProphetService.OnCmdUpdateTimer(Context, conn, cmd));
        //别名列表指令
        Context.Handlers.RegisterHandler("aliases", (conn, cmd) => ProphetService.OnCmdAliases(Context, conn, cmd));
        //创建别名指令
        Context.Handlers.RegisterHandler("createAlias", (conn, cmd) => ProphetService.OnCmdCreateAlias(Context, conn, cmd));
        //删除别名指令
        Context.Handlers.RegisterHandler("deleteAlias", (conn, cmd) => ProphetService.OnCmdDeleteAlias(Context, conn, cmd));
        //获取单个别名指令
        Context.Handlers.RegisterHandler("loadAlias", (conn, cmd) => ProphetService.OnCmdLoadAlias(Context, conn, cmd));
        //更新别名指令
        Context.Handlers.RegisterHandler("updateAlias", (conn, cmd) => ProphetService.OnCmdUpdateAlias(Context, conn, cmd));
        //触发器列表指令
        Context.Handlers.RegisterHandler("triggers", (conn, cmd) => ProphetService.OnCmdTriggers(Context, conn, cmd));
        //创建触发器指令
        Context.Handlers.RegisterHandler("createTrigger", (conn, cmd) => ProphetService.OnCmdCreateTrigger(Context, conn, cmd));
        //删除触发器指令
        Context.Handlers.RegisterHandler("deleteTrigger", (conn, cmd) => ProphetService.OnCmdDeleteTrigger(Context, conn, cmd));
        //加载单个触发器指令
        Context.Handlers.RegisterHandler("loadTrigger", (conn, cmd) => ProphetService.OnCmdLoadTrigger(Context, conn, cmd));
        //更新触发器指令
        Context.Handlers.RegisterHandler("updateTrigger", (conn, cmd) => ProphetService.OnCmdUpdateTrigger(Context, conn, cmd));
        //更新密码指令
        Context.Handlers.RegisterHandler("updatepassword", (conn, cmd) => ProphetService.OnCmdUpdatePassword(Context, conn, cmd));
        //查找历史指令，已废弃
        Context.Handlers.RegisterHandler("findhistory", (conn, cmd) => ProphetService.OnCmdFindHistory(Context, conn, cmd));
        //列出变量指令
        Context.Handlers.RegisterHandler("params", (conn, cmd) => ProphetService.OnCmdParams(Context, conn, cmd));
        //更新变量指令
        Context.Handlers.RegisterHandler("updateParam", (conn, cmd) => ProphetService.OnCmdUpdateParam(Context, conn, cmd));
        //删除变量指令
        Context.Handlers.RegisterHandler("deleteParam", (conn, cmd) => ProphetService.OnCmdDeleteParam(Context, conn, cmd));
        //更新变量备注指令
        Context.Handlers.RegisterHandler("updateParamComment", (conn, cmd) => ProphetService.OnCmdUpdateParamComment(Context, conn, cmd));
        //指定回调指令
        Context.Handlers.RegisterHandler("callback", (conn, cmd) => ProphetService.OnCmdCallback(Context, conn, cmd));
        //调用助理按钮对应功能指令
        Context.Handlers.RegisterHandler("assist", (conn, cmd) => ProphetService.OnCmdAssist(Context, conn, cmd));
        //显示服务器介绍信息指令
        Context.Handlers.RegisterHandler("about", (conn, cmd) => ProphetService.OnCmdAbout(Context, conn, cmd));
        //请求游戏信息指令
        Context.Handlers.RegisterHandler("worldSettings", (conn, cmd) => ProphetService.OnCmdWorldSettings(Context, conn, cmd));
        //请求脚本信息指令
        Context.Handlers.RegisterHandler("scriptSettings", (conn, cmd) => ProphetService.OnCmdScriptSettings(Context, conn, cmd));
        //请求脚本参数列表指令
        Context.Handlers.RegisterHandler("requiredParams", (conn, cmd) => ProphetService.OnCmdRequiredParams(Context, conn, cmd));
        //更新脚本参数指令
        Context.Handlers.RegisterHandler("updateRequiredParams", (conn, cmd) => ProphetService.OnCmdUpdateRequiredParams(Context, conn, cmd));
        //更新游戏设置指令;
        Context.Handlers.RegisterHandler("updateWorldSettings", (conn, cmd) => ProphetService.OnCmdUpdateWorldSettings(Context, conn, cmd));
        //更新脚本信息指令
        Context.Handlers.RegisterHandler("updateScriptSettings", (conn, cmd) => ProphetService.OnCmdUpdateScriptSettings(Context, conn, cmd));
        //请求默认服务器指令
        Context.Handlers.RegisterHandler("defaultServer", (conn, cmd) => ProphetService.OnCmdDefaultServer(Context, conn, cmd));
        //请求默认编码指令
        Context.Handlers.RegisterHandler("defaultCharset", (conn, cmd) => ProphetService.OnCmdDefaultCharset(Context, conn, cmd));
        //授权指令
        Context.Handlers.RegisterHandler("requestPermissions", (conn, cmd) => ProphetService.OnCmdRequestPermissions(Context, conn, cmd));
        //授权信任域名指令
        Context.Handlers.RegisterHandler("requestTrustDomains", (conn, cmd) => ProphetService.OnCmdRequestTrustDomains(Context, conn, cmd));
        //请求已授权信息指令
        Context.Handlers.RegisterHandler("authorized", (conn, cmd) => ProphetService.OnCmdAuthorized(Context, conn, cmd));
        //注销已授权内容指令;
        Context.Handlers.RegisterHandler("revokeAuthorized", (conn, cmd) => ProphetService.OnCmdRevokeAuthorized(Context, conn, cmd));
        //批量发送文本指令
        Context.Handlers.RegisterHandler("masssend", (conn, cmd) => ProphetService.OnCmdMasssend(Context, conn, cmd));
        //HUD被点击指令
        Context.Handlers.RegisterHandler("hudclick", (conn, cmd) => ProphetService.OnCmdHUDClick(Context, conn, cmd));
        //对客户端进行排序指令
        Context.Handlers.RegisterHandler("sortclients", (conn, cmd) => ProphetService.OnCmdSortClients(Context, conn, cmd));
        //用户按键指令
        Context.Handlers.RegisterHandler("keyup", (conn, cmd) => ProphetService.OnCmdKeyUp(Context, conn, cmd));
        //批量发送指令
        Context.Handlers.RegisterHandler("batchcommand", (conn, cmd) => ProphetService.OnCmdBatchCommand(Context, conn, cmd));
        //获取批量发送信息指令
        Context.Handlers.RegisterHandler("batchcommandscripts", (conn, cmd) => ProphetService.OnCmdBatchCommandScripts(Context, conn, cmd));
    }
}