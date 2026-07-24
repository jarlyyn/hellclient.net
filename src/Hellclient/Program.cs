using Hellclient.World.Infras.Components;
using Hellclient.World.Types;
using Hellclient.World.Cores;
using Hellclient.World.Utils;
using Hellclient.Core.Utils;
using Hellclient.Application;

Application.Instance.Init();
Application.Instance.Config();

CharsetUtil.InstallEncodingProvider();
var world = Core.Instance.WorldManager.NewWorld("test");
world.SetCharset(CharsetUtil.GBK);
world.SetHost(Environment.GetEnvironmentVariable("TEST_HOST")!);
world.SetPort(Environment.GetEnvironmentVariable("TEST_PORT")!);


world.EventBus.LineEvent += (sender, data) =>
{
    Console.WriteLine(data.ToPlainText());
};
await world.DoConnectServer();
// world.Context.Connection.OnCommandReceived += (sender, cmd) =>
// {
//     Console.WriteLine($"Command received: {cmd.Command}, Data: {BitConverter.ToString(cmd.Data)}");
// };
// world.Context.Connection.OnConnected += (sender, e) =>
// {
//     Console.WriteLine("Connected to the server.");
// };
// world.Context.Connection.OnDisconnected += (sender, e) =>
// {
//     Console.WriteLine("Disconnected from the server.");
// };
// world.DoConnectServer();
await world.DoSend(Command.Create(Environment.GetEnvironmentVariable("TEST_ID")!));
await world.DoSend(Command.Create(Environment.GetEnvironmentVariable("TEST_PASS")!));
await world.DoSend(Command.Create("y"));
// Thread.Sleep(1000);
// await world.DoSend("name\r\n");
// Thread.Sleep(1000);
// await world.DoCloseServer();
// await world.DoSend("passwd\r\n");

Thread.Sleep(10000000);

