using Tomlyn.Serialization;
using Hellclient.World.Types;
using System.Text.Json.Serialization;

namespace Hellclient.World.Infras.Components;

[TomlSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.Unspecified,
    PreferredObjectCreationHandling = JsonObjectCreationHandling.Replace)]

[TomlSerializable(typeof(SystemConfig))]
internal partial class TomlContext : TomlSerializerContext
{
}