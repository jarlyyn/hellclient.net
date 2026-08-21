# 架构说明

Hellclient.net是Hellclient(golang)的重写，主要是为了解决v8库的后续更新问题，对架构也做了一定合理化的调整。

## 基础

hc.net把项目分为了多个子项目。每个项目中的从上向下架构大体如下

* Core 核心层，子项目中的 核心接口，本质是将 功能的上下文和函数做一个封装，作为一个统一稳定的对外接口暴露出来
* Features 功能层。分别包含Services(处理功能代码),Repos(持久化读取),States(状态层，主要是各种业务本体的数据/状态的上下文)
* Infras 基础设施层，包含Adaptor(适配器)和组件(Componets),组件指同时包含数据和功能，但比较简单或者与核心功能无关的迷你系统。
* Helper 帮助函数，基础处理
* Types 基础结构/接口/常量
* Utils 基础工具。

其中Service中每个Service对应一个接口，作为功能约定。所以查看代码时会多需要跳转一层，Cores中的代码都是带上上下文直接转发到服务的接口，因此一个调用会有4个代码(接口/实现-带上下文/不带上下文)。这是个人故意的设计。

代码都在项目的/src目录下

## 子项目划分

* Hellclient.World 具体的一个一个游戏的实例。包含了telnet连接，ansi转义，内部Line对象的转换和管理，触发器/别名/计时器功能，各种事件和API提供控制
* Hellclient.Script 在World之上的，定义控制World的API，可以注入到各个脚本引擎内实现脚本功能。比如Hellclient.V8Engine,就是javascript(V8)的实现
* Hellclient.Core 核心层，主要是3个内容，Titan,Prophet,Messenger。Titan(天神)负责管理调度所有的world,Go中单例叫做盘古。Prophet(先知)负责沟通最终用户和Titan，Go中的单例叫做老子。Messenger(信使)负责其他程序通过Websocket协议与客户端进行协作.Go中的实例叫做太白金星。
* Hellcient 入口层，维护了UI(目前只有UI,理论上对接Avalonia等gui),进行生命周期和依赖注入。

## 数据流转

入方向的数据为

Telnet原始数据->AnsiHelper解析为Line(游戏内核心数据)->InfoService进行管理->抛出事件->Titan通过MsgHelper Publish为指令->Prophet进行Json序列化，丢入连接Adappter->通过Websocket传递给Web界面/App/客户端

出方向数据为

用户在界面进行操作->Websocket发送指令->Prophet接受指令后，Json反序列化，通过Hanlders进行转发->Titan接受指令，获取对应的World,调用接口->World调用Script引擎或者Alias->发送到telnet或者显示到输出。