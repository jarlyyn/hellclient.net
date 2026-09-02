using Hellclient.Core.Types;
using Hellclient.Core.Infras.Components;
using Hellclient.Core.Features.Services;

namespace Hellclient.Core.Cores;

public partial class Prophet
{
    private class ProphetAdapter(Prophet p, string cmdtype)
    {
        public Prophet Prophet { get; init; } = p;
        public string CmdType { get; init; } = cmdtype;

        public void RoomAdapter(Message m)
        {
            if (m.Room != "" && m.Room == Prophet.GetCurrent())
            {
                var data = new SeparatedCommand();
                data.CommandType = CmdType;
                data.CommandData = JsonContext.Serialize(m.Data);
                var msg = data.Encode();
                Prophet.SendToUser(msg);
            }
        }
        public void ConsoleAdapter(Message m)
        {
            if (m.Room != "" && m.Room == Prophet.GetCurrent())
            {
                var data = new SeparatedCommand();
                data.CommandType = CmdType;
                data.CommandData = JsonContext.Serialize(m.Data);
                var msg = data.Encode();
                Prophet.SendToUser(msg);
            }
        }
        public void UserAdapter(Message m)
        {
            if (m.Room == "")
            {
                var data = new SeparatedCommand();
                data.CommandType = CmdType;
                data.CommandData = JsonContext.Serialize(m.Data);
                var msg = data.Encode();
                Prophet.SendToUser(msg);
            }
        }
    }

    private Action<Message> newRoomAdapter(string cmdtype)
    {
        return new ProphetAdapter(this, cmdtype).RoomAdapter;
    }
    private Action<Message> newConsoleAdapter(string cmdtype)
    {
        return new ProphetAdapter(this, cmdtype).ConsoleAdapter;
    }
    private Action<Message> newUserAdapter(string cmdtype)
    {
        return new ProphetAdapter(this, cmdtype).UserAdapter;
    }

    public void initAdapter()
    {
        // 只有在这里注册过的信息才会发送给客户端
        // 注册的适配器与 /src/world/msg/msg,go中的msgtype对应,由msg.go负责序列化为标准信息
        // 适配器负责个根据当前连接状态决定是否将信息发送给客户端。
        // 具体与客户端的交互可以参考 /resources/public/defaultui/js/handlers.js下的handler.[适配器名] 函数
        // newRoomAdapter 为发送给当前游戏
        // newUserAdapter 为发送给客户端，无视游戏
        Context.Adapter.RegisterHandler("line", newRoomAdapter("line"));
        //多行信息，预期客户端清空主输出并更新内容
        Context.Adapter.RegisterHandler("lines", newRoomAdapter("lines"));
        //提示行更新，预期更新客户端提示行部分
        Context.Adapter.RegisterHandler("prompt", newRoomAdapter("prompt"));
        //触发列表，预期客户端弹出触发器列表
        Context.Adapter.RegisterHandler("triggers", newRoomAdapter("triggers"));
        //客户端信息，预期客户端更新所有已经打开的客户端的信息
        Context.Adapter.RegisterHandler("clients", newUserAdapter("clients"));
        //连线信息，通知客户端更新已经连线状态
        Context.Adapter.RegisterHandler("connected", newUserAdapter("connected"));
        //断线信息，通知客户端更新已经连线状态
        Context.Adapter.RegisterHandler("disconnected", newUserAdapter("disconnected"));
        //创建失败的错误信息，预期客户端提示错误
        Context.Adapter.RegisterHandler("createFail", newUserAdapter("createFail"));
        //创建成功，预期客户端关闭创建窗口
        Context.Adapter.RegisterHandler("createSuccess", newUserAdapter("createSuccess"));
        //更新成功，预期客户端关闭更新窗口
        Context.Adapter.RegisterHandler("updateSuccess", newUserAdapter("updateSuccess"));
        //触发维护失败，预期客户端提示错误
        Context.Adapter.RegisterHandler("triggerFail", newRoomAdapter("triggerFail"));
        //触发维护成功，预期客户端关闭更新窗口
        Context.Adapter.RegisterHandler("triggerSuccess", newRoomAdapter("triggerSuccess"));
        //历史信息，预期客户端弹出历史信息界面
        Context.Adapter.RegisterHandler("allLines", newRoomAdapter("allLines"));
        //未打开游戏列表，预期客户端弹出打开游戏界面
        Context.Adapter.RegisterHandler("notopened", newUserAdapter("notopened"));
        //脚本信息，预期客户端弹出脚本信息详情
        Context.Adapter.RegisterHandler("scriptinfo", newRoomAdapter("scriptinfo"));
        //创建脚本失败，预期客户端提示用户错误信息
        Context.Adapter.RegisterHandler("createScriptFail", newUserAdapter("createScriptFail"));
        //创建脚本成功，预期客户端关闭创建界面
        Context.Adapter.RegisterHandler("createScriptSuccess", newUserAdapter("createScriptSuccess"));
        //更新脚本成功，预期客户端关闭更新界面
        Context.Adapter.RegisterHandler("updateScriptSuccess", newUserAdapter("updateScriptSuccess"));
        //脚本列表，预期客户端弹出脚本列表，并选择游戏脚本
        Context.Adapter.RegisterHandler("scriptinfoList", newUserAdapter("scriptinfoList"));
        //更新状态行，一般在脚本SetStatus后更新，预期客户端更新状态行
        Context.Adapter.RegisterHandler("status", newRoomAdapter("status"));
        //输入历史更新，预期客户端更新历史信息/补全信息。
        Context.Adapter.RegisterHandler("history", newRoomAdapter("history"));
        //用户计时器信息列表，预期客户端弹出用户计时器列表
        Context.Adapter.RegisterHandler("usertimers", newRoomAdapter("usertimers"));
        //脚本时器信息列表，预期客户端弹出脚本计时器列表
        Context.Adapter.RegisterHandler("scripttimers", newRoomAdapter("scripttimers"));
        //创建计时器成功，预期客户端关闭计时器创建界面
        Context.Adapter.RegisterHandler("createTimerSuccess", newRoomAdapter("createTimerSuccess"));
        //计时器详情，预期客户端弹出计时器详情
        Context.Adapter.RegisterHandler("timer", newRoomAdapter("timer"));
        //更新计时器成功，预期客户端关闭计时器更新界面
        Context.Adapter.RegisterHandler("updateTimerSuccess", newRoomAdapter("updateTimerSuccess"));
        //用户别名列表，预期客户端弹出用户别名列表界面
        Context.Adapter.RegisterHandler("useraliases", newRoomAdapter("useraliases"));
        //脚本别名列表，预期客户端弹出脚本别名列表界面
        Context.Adapter.RegisterHandler("scriptaliases", newRoomAdapter("scriptaliases"));
        //创建别名成功，预期客户端会关闭创建别名窗口
        Context.Adapter.RegisterHandler("createAliasSuccess", newRoomAdapter("createAliasSuccess"));
        //别名详情，预期客户端会弹出别名详情
        Context.Adapter.RegisterHandler("alias", newRoomAdapter("alias"));
        //更新别名成功，预期客户端会关闭更新别名差窗口
        Context.Adapter.RegisterHandler("updateAliasSuccess", newRoomAdapter("updateAliasSuccess"));
        //用户触发器列表，预期客户端弹出用户触发器列表界面
        Context.Adapter.RegisterHandler("usertriggers", newRoomAdapter("usertriggers"));
        //脚本触发器列表，预期客户端弹出脚本触发器列表界面
        Context.Adapter.RegisterHandler("scripttriggers", newRoomAdapter("scripttriggers"));
        //创建触发器成功，预期客户端关闭创建触发器界面
        Context.Adapter.RegisterHandler("createTriggerSuccess", newRoomAdapter("createTriggerSuccess"));
        //触发器详情，预期客户端弹出触发器详情
        Context.Adapter.RegisterHandler("trigger", newRoomAdapter("trigger"));
        //更新触发器成功，预期客户端关闭更新触发器界面
        Context.Adapter.RegisterHandler("updateTriggerSuccess", newRoomAdapter("updateTriggerSuccess"));
        //变量列表，预期客户端弹出变量界面
        Context.Adapter.RegisterHandler("paramsinfo", newRoomAdapter("paramsinfo"));
        //变量更新成功，预期客户端关闭更新变量界面
        Context.Adapter.RegisterHandler("paramupdated", newRoomAdapter("paramupdated"));
        //变量删除成功，预期客户端维护变量界面
        Context.Adapter.RegisterHandler("paramdeleted", newRoomAdapter("paramdeleted"));
        //变量备注更新成功，预期客户端关闭变量备注界面
        Context.Adapter.RegisterHandler("paramcommentupdated", newRoomAdapter("paramcommentupdated"));
        //脚本信息(userinput),预期客户端根据信息内容与用户进行交互
        Context.Adapter.RegisterHandler("scriptMessage", newRoomAdapter("scriptMessage"));
        //交换机状态，预期客户端更新交换机信息
        Context.Adapter.RegisterHandler("switchStatus", newUserAdapter("switchStatus"));
        //版本信息，预期客户端弹出版本信息界面
        Context.Adapter.RegisterHandler("version", newUserAdapter("version"));
        //API信息，连接时发送，预期客户端根据API版本显示不同界面
        Context.Adapter.RegisterHandler("apiversion", newUserAdapter("apiversion"));
        //游戏信息界面，预期客户端弹出游戏信息
        Context.Adapter.RegisterHandler("worldSettings", newRoomAdapter("worldSettings"));
        //脚本信息界面，预期客户端弹出脚本信息
        Context.Adapter.RegisterHandler("scriptSettings", newRoomAdapter("scriptSettings"));
        //游戏变量界面，预期客户端显示游戏变量维护界面
        Context.Adapter.RegisterHandler("requiredParams", newRoomAdapter("requiredParams"));
        //默认服务器信息，预期客户端在创建游戏时填入默认服务器信息
        Context.Adapter.RegisterHandler("defaultServer", newUserAdapter("defaultServer"));
        //默认编码信息，预期客户端在创建游戏时填入默认编码信息
        Context.Adapter.RegisterHandler("defaultCharset", newUserAdapter("defaultCharset"));
        //获取支持的脚本类型，在创建脚本是使用
        Context.Adapter.RegisterHandler("scriptTypes", newUserAdapter("scriptTypes"));
        //授权请求，预期客户端弹授权界面
        Context.Adapter.RegisterHandler("requestPermissions", newRoomAdapter("requestPermissions"));
        //授权域名信息，预期客户端弹出授权域名界面
        Context.Adapter.RegisterHandler("requestTrustDomains", newRoomAdapter("requestTrustDomains"));
        //已授权界面，预期客户端弹出已授权内容界面
        Context.Adapter.RegisterHandler("authorized", newRoomAdapter("authorized"));
        //查找历史信息，已废弃
        Context.Adapter.RegisterHandler("foundhistory", newRoomAdapter("foundhistory"));
        //HUD全量更新，预期客户端更新完整的HUD信息
        Context.Adapter.RegisterHandler("hudcontent", newRoomAdapter("hudcontent"));
        //HUD部分更新，预期客户端更新HUD中的指定行
        Context.Adapter.RegisterHandler("hudupdate", newRoomAdapter("hudupdate"));
        //客户端信息，预期客户端更新客户端界面中的单个客户端信息
        Context.Adapter.RegisterHandler("clientinfo", newConsoleAdapter("clientinfo"));
        //批量执行脚本信息，预期客户端弹出批量执行脚本界面
        Context.Adapter.RegisterHandler("batchcommandscripts", newUserAdapter("batchcommandscripts"));
    }
}
