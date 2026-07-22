using Hellclient.World.Infras.Components;
using Hellclient.World.Types;
using Hellclient.World.Cores;
using Hellclient.World.Utils;
using Hellclient.Core.Utils;
using Hellclient.Application;

Application.Instance.Init();
Application.Instance.Config();

CharsetUtil.InstallEncodingProvider();
var world=new World("test");
world.Context.Data.Charset = CharsetUtil.GBK;
world.Context.Convert.OnLine += (sender, data) =>
{
    Console.WriteLine( data.ToPlainText());
};
world.Context.Connection.OnCommandReceived += (sender, cmd) =>
{
    Console.WriteLine($"Command received: {cmd.Command}, Data: {BitConverter.ToString(cmd.Data)}");
};
world.Context.Connection.OnConnected += (sender, e) =>
{
    Console.WriteLine("Connected to the server.");
};
world.Context.Connection.OnDisconnected += (sender, e) =>
{
    Console.WriteLine("Disconnected from the server.");
};
world.SetHost(Environment.GetEnvironmentVariable("TEST_HOST")!);
world.SetPort(Environment.GetEnvironmentVariable("TEST_PORT")!);
world.DoConnectServer();

Thread.Sleep(1000);
await world.Context.Connection.Send(System.Text.Encoding.UTF8.GetBytes("name\r\n"));
Thread.Sleep(1000);
await world.Context.Connection.Disconnect();
await world.Context.Connection.Send(System.Text.Encoding.UTF8.GetBytes("passwd\r\n"));

Thread.Sleep(10000000);
