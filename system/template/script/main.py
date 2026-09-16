

# 初始化节拍器发送
Metronome.SetTick(600)
Metronome.SetBeats(10)

#Metronome.Push(["cmd1","cmd2","cmd3","cmd4"],False,True)

# 客户端API的封装，可以注入到子脚本对象中
class GameAPI:
    World =world #游戏API
    Metronome=Metronome #节拍器API
    Userinput=Userinput #用户界面操作

def onOpen():
    pass
def onClose():
    pass
def onConnected():
    pass
def onDisconnected():
    pass
def onAssist():
    pass
def onBroadcast(msg,isglobal,channel):
    pass
def onResponse(type,id,data):
    pass
def onKeyup(key):
    pass
def onLine(text,ansi):
    return False
def onAfterLine(text,ansi):
    pass
def onSend(text):
    return False

def onTimer(name):
    pass
def onTrigger(name,line,wildcarts):
    pass

def onCallback(name, id, code, data):
    pass