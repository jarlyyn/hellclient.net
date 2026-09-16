# 如何使用python脚本

python脚本通过 c#的[python.net库](https://pythonnet.github.io/)实现。

具体的python版本支持取决于程序使用的python.net库版本。

## 设置Python 环境

要使用python，首先需要在config/system.toml里加入相应的环境变量

```
PYTHONNET_PYDLL="/usr/lib/x86_64-linux-gnu/libpython3.13.so"
```
填入正确的 python的dll/so文件位置

如果需要指定pythonpath,比如需要将你的脚本加入path,需要定义 PythonPah环境变量，如
```
PYTHONPATH="/usr/lib/python313.zip:/usr/lib/python3.13:/usr/lib/python3.13/lib-dynload:/home/XXXX/hellclient.net/appdata/game/scripts/pytest/script/"
```

在第一次加载使用python脚本时，会在控制台打印出当前的PythonPath信息,方便打印。

## API接口

客户端会在脚本的入口文件的命名空间引入以下的API:
* world 游戏API
* Metronome 节拍器(发送限流)API

建议封装在一个类里，以依赖注入的形式供子包使用。