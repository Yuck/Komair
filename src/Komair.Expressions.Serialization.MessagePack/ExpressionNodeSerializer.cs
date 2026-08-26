using System.Buffers;
using Komair.Expressions.Abstract;
using Komair.Expressions.Serialization.Abstract.Interfaces;
using Komair.Expressions.Serialization.Exceptions;
using Komair.Expressions.Serialization.MessagePack.Internal;
using MessagePack;
using MessagePack.Resolvers;

namespace Komair.Expressions.Serialization.MessagePack;

/// <summary>
/// Serializes and deserializes <typeparamref name="TExpressionNode"/> instances using MessagePack.
/// </summary>
/// <typeparam name="TExpressionNode">The concrete expression node root type.</typeparam>
/// <param name="options">Optional MessagePack serializer options; defaults are used when <see langword="null"/>.</param>
public class ExpressionNodeSerializer<TExpressionNode>(MessagePackSerializerOptions? options = null) : IExpressionNodeSerializer<Byte[], TExpressionNode> where TExpressionNode : ExpressionNodeBase
{
    private readonly MessagePackSerializerOptions _options = CreateOptions(options);

    /// <inheritdoc />
    public TExpressionNode Deserialize(Byte[] document)
    {
        ArgumentNullException.ThrowIfNull(document);

        try
        {
            var reader = new MessagePackReader(document);

            if (reader.TryReadNil())
                throw new ExpressionSerializationException($"Failed to deserialize {typeof(TExpressionNode).Name} from MessagePack.");

            if (reader.ReadArrayHeader() != 2)
                throw new ExpressionSerializationException("Expected MessagePack root to be a 2-element envelope array of [schema, node].");

            var schemaVersion = reader.ReadInt32();

            if (schemaVersion > ExpressionSerializationWireFormat.CurrentSchemaVersion)
                throw new ExpressionSerializationException($"Unsupported expression serialization schema version {schemaVersion}; maximum supported version is {ExpressionSerializationWireFormat.CurrentSchemaVersion}.");

            if (schemaVersion != ExpressionSerializationWireFormat.CurrentSchemaVersion)
                throw new ExpressionSerializationException($"Unsupported expression serialization schema version {schemaVersion}; migrate stored payloads or use a serializer that supports that schema.");

            var result = MessagePackSerializer.Deserialize<ExpressionNodeBase>(ref reader, _options);

            if (result is null)
                throw new ExpressionSerializationException($"Failed to deserialize {typeof(TExpressionNode).Name} from MessagePack.");

            if (result is not TExpressionNode typed)
                throw new ExpressionSerializationException($"Failed to deserialize {typeof(TExpressionNode).Name} from MessagePack.");

            return typed;
        }
        catch (ExpressionSerializationException)
        {
            throw;
        }
        catch (Exception exception) when (exception is MessagePackSerializationException or InvalidOperationException or ArgumentException or EndOfStreamException)
        {
            throw new ExpressionSerializationException($"Failed to deserialize {typeof(TExpressionNode).Name} from MessagePack.", exception);
        }
    }

    /// <inheritdoc />
    public Byte[] Serialize(TExpressionNode node)
    {
        ArgumentNullException.ThrowIfNull(node);

        try
        {
            var buffer = new ArrayBufferWriter<Byte>();
            var writer = new MessagePackWriter(buffer);

            writer.WriteArrayHeader(2);
            writer.Write(ExpressionSerializationWireFormat.CurrentSchemaVersion);
            MessagePackSerializer.Serialize(ref writer, (ExpressionNodeBase) node, _options);
            writer.Flush();

            return buffer.WrittenSpan.ToArray();
        }
        catch (Exception exception) when (exception is MessagePackSerializationException or InvalidOperationException or ArgumentException)
        {
            throw new ExpressionSerializationException($"Failed to serialize {typeof(TExpressionNode).Name} to MessagePack.", exception);
        }
    }

    private static MessagePackSerializerOptions CreateOptions(MessagePackSerializerOptions? options)
    {
        var resolver = CompositeResolver.Create([new ExpressionNodeBaseFormatter(), new TypeFormatter()], [ContractlessStandardResolver.Instance]);

        if (options is null)
            return MessagePackSerializerOptions.Standard.WithResolver(resolver);

        return options.WithResolver(CompositeResolver.Create([new ExpressionNodeBaseFormatter(), new TypeFormatter()], [options.Resolver]));
    }
}
