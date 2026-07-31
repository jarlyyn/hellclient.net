namespace Hellclient.Core.Features.States;

public class ClientContext
{
    public TitanContext Titan{ get; set; } = new TitanContext();
    public ProphetContext Prophet{ get; set; } = new ProphetContext();
}

