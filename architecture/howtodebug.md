# Debug说明

开发者本人使用的linux+vscode，但由于微软本身的强大技术实力，不管什么系统下进行debug都是比较容易的。

## 安装dotnet runtime

在微软官方网站下载并安装

https://dotnet.microsoft.com/zh-cn/download

## 安装vscode

官网地址

https://code.visualstudio.com/

## 安装c#插件

官网地址

https://learn.microsoft.com/zh-cn/visualstudio/subscriptions/vs-c-sharp-dev-kit

## 调试运行

首先需要在vscode中信任你下载回来的源码，当心有问题可以先用AI过一遍。

在vscode中按f5或者选择 运行和调试，选择 c#,再选择 c#:hellclient,齐活

## 打包发布

在 src目录下，dotnet publish,然后可执行文件会生成在对应操作系统和架构的默认位置

新建一个目录，把编译生成的可执行文件和v8 dll/so 放在  bin目录下

把resources和system目录也复制进去，就完成了。