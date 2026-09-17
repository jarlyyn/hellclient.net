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

## 资源释放

由于python在一个进程里默认只有一个实例(GIL),以及python.net库不支持子解释器功能。所以所有的python脚本实际上是在同一个解释器范围内运行的。每个不同的游戏只是有一个独立的初始空间(PythonScope)。除了入口的脚本空间之外所有的package/module是所有游戏共享的。

重载游戏也只是重新初始化游戏对应的初始空间。所以无法通过重载游戏/脚本进行资源释放。所以在使用资源时请注意资源的释放。

建议资源不要零散使用，可以使用一个单独的package,根据游戏id进行统一管理和释放。

## 后台线程

在客户端调用脚本时，脚本执行结束时对应的时间循环都会释放。

所以如果有后台线程的需要，需要手动创建一个线程并设置eventloop

```python
def _start_background_loop(loop):
    asyncio.set_event_loop(loop)
    loop.run_forever()
_loop = asyncio.new_event_loop()
_thread = threading.Thread(target=_start_background_loop, args=(_loop,), daemon=True)
_thread.start()
asyncio.run_coroutine_threadsafe(timer.start(), _loop)
```

但处于多python游戏资源管理的复杂度，并不是很建议使用这种方式。

特别注意。

相对而言，可以认为 hellclient.net和 python空间是完全独立的两个程序，只是因为在一个进程内，可以很有效率的进行进程内通信互相调用。

hellclient.net支持传递一个代表当前游戏的命名空间，以及绑定到当前游戏的一些API给python，执行代码，对python内容没有任何控制力。

在python端创建的后台线程，当前版本，在前台游戏关闭后，甚至所有游戏都关闭后，依然会持续保持运行，无法从hellclient.net程序内进行控制，需要严格的在python端进行控制。

## 模块更新

在进行代码维护时，经常需要把代码放在子包内，并经常重新加载最新的子包。

由于上述的所有游戏共享内存空间的原因，重新加载代码无法进行全局的代码种加载，需要子代码里手动指定的进行处理。

范例代码：
```python
import importlib
import my_module  # Must be imported first
# Make changes to my_module.py externally...
# Reload the module to apply changes
importlib.reload(my_module)
```