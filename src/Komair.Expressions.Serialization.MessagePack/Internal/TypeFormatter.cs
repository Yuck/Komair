using MessagePack;
using MessagePack.Formatters;

namespace Komair.Expressions.Serialization.MessagePack.Internal;

/// <summary>
/// Serializes <see cref="Type"/> as an assembly-qualified name string.
/// </summary>
internal sealed class TypeFormatter : IMessagePackFormatter<Type?>
{
    /// <inheritdoc />
    public Type? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        if (reader.TryReadNil())
            return null;

        var typeName = reader.ReadString();
        if (String.IsNullOrWhiteSpace(typeName))
            return null;

        return Type.GetType(typeName, throwOnError: true);
    }

    /// <inheritdoc />
    public void Serialize(ref MessagePackWriter writer, Type? value, MessagePackSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNil();

            return;
        }

        writer.Write(value.AssemblyQualifiedName);
    }
}
