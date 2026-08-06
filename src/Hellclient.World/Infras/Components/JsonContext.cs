using System.Text.Json.Serialization;
using Hellclient.World.Types;

namespace Hellclient.World.Infras.Components;

[JsonSerializable(typeof(BatchCommand))]
public partial class JsonContext : JsonSerializerContext
{
    
}
