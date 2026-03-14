using System.Text.Json;
using System.Text.Json.Serialization;

namespace Komair.Expressions.Serialization.Json.Internal;

internal sealed class TypeJsonConverter : JsonConverter<Type>
{
    public override Type? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var type = reader.GetString();
        var result = ! String.IsNullOrWhiteSpace(type) ? Type.GetType(type, throwOnError: true) : null;

        return result;
    }

    public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.AssemblyQualifiedName);
    }
}
