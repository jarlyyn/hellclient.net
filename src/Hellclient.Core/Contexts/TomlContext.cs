using Tomlyn.Serialization;
using Hellclient.Core.Models;
using System.Text.Json.Serialization;

namespace Hellclient.Core.Contexts;

[TomlSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.Unspecified,
    PreferredObjectCreationHandling = JsonObjectCreationHandling.Replace)]

[TomlSerializable(typeof(SystemConfig))]
internal partial class TomlContext : TomlSerializerContext
{
}