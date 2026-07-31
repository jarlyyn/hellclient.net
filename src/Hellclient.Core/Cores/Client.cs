using Hellclient.Core.Features.States;

namespace Hellclient.Core.Cores;

public class Client
{
    public required Titan Titan { get; init; }
    public required ClientContext Context { private get; init; }
}