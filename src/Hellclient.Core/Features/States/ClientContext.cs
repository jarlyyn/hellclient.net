namespace Hellclient.Core.Features.States;

public class ClientContext
{
    public required TitanContext Titan{ get; set; }
    public required ProphetContext Prophet{ get; set; }
}

