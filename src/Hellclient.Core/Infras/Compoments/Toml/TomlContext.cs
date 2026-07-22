using Tomlyn.Serialization;
using Hellclient.Core.Types;
using System.Text.Json.Serialization;

namespace Hellclient.Core.Infras.Compoments.Toml;

[TomlSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.Unspecified,
    PreferredObjectCreationHandling = JsonObjectCreationHandling.Replace)]

[TomlSerializable(typeof(SystemConfig))]
internal partial class TomlContext : TomlSerializerContext
{
}