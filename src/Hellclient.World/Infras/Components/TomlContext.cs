using Tomlyn.Serialization;
using Hellclient.World.Types;
using Hellclient.World.Infras.Components;
using System.Text.Json.Serialization;

namespace Hellclient.World.Infras.Components;

[TomlSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.Unspecified,
    PreferredObjectCreationHandling = JsonObjectCreationHandling.Replace)]

[TomlSerializable(typeof(SystemConfig))]
[TomlSerializable(typeof(ScriptData))]
[TomlSerializable(typeof(WorldData))]
public partial class TomlContext : TomlSerializerContext
{
}