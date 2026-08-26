using System.Buffers;
using System.Linq.Expressions;
using Komair.Expressions.Abstract;
using MessagePack;
using MessagePack.Formatters;

namespace Komair.Expressions.Serialization.MessagePack.Internal;

/// <summary>
/// Serializes polymorphic <see cref="ExpressionNodeBase"/> graphs with string discriminators matching the JSON serializer.
/// </summary>
internal sealed class ExpressionNodeBaseFormatter : IMessagePackFormatter<ExpressionNodeBase?>
{
    private const String DiscriminatorBinary = "Binary";
    private const String DiscriminatorBlock = "Block";
    private const String DiscriminatorConditional = "Conditional";
    private const String DiscriminatorConstant = "Constant";
    private const String DiscriminatorInvocation = "Invocation";
    private const String DiscriminatorLambda = "Lambda";
    private const String DiscriminatorListInit = "ListInit";
    private const String DiscriminatorMember = "Member";
    private const String DiscriminatorMemberInit = "MemberInit";
    private const String DiscriminatorNew = "New";
    private const String DiscriminatorParameter = "Parameter";
    private const String DiscriminatorQuote = "Quote";

    private const String KeyAddMethodName = "AddMethodName";
    private const String KeyArguments = "Arguments";
    private const String KeyBindings = "Bindings";
    private const String KeyBody = "Body";
    private const String KeyConstructorParameterTypes = "ConstructorParameterTypes";
    private const String KeyExpression = "Expression";
    private const String KeyExpressions = "Expressions";
    private const String KeyIfFalse = "IfFalse";
    private const String KeyIfTrue = "IfTrue";
    private const String KeyInitializers = "Initializers";
    private const String KeyLeft = "Left";
    private const String KeyMemberName = "MemberName";
    private const String KeyMemberNames = "MemberNames";
    private const String KeyName = "Name";
    private const String KeyNewExpression = "NewExpression";
    private const String KeyNodeType = "NodeType";
    private const String KeyOperand = "Operand";
    private const String KeyParameters = "Parameters";
    private const String KeyRight = "Right";
    private const String KeyTest = "Test";
    private const String KeyType = "Type";
    private const String KeyValue = "Value";
    private const String KeyVariables = "Variables";

    /// <inheritdoc />
    public ExpressionNodeBase? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        if (reader.TryReadNil())
            return null;

        if (reader.ReadArrayHeader() != 2)
            throw new MessagePackSerializationException("Expression node must be a 2-element array of [discriminator, payload].");

        var discriminator = reader.ReadString();

        return discriminator switch
        {
            DiscriminatorBinary => DeserializeBinary(ref reader, options),
            DiscriminatorBlock => DeserializeBlock(ref reader, options),
            DiscriminatorConditional => DeserializeConditional(ref reader, options),
            DiscriminatorConstant => DeserializeConstant(ref reader, options),
            DiscriminatorInvocation => DeserializeInvocation(ref reader, options),
            DiscriminatorLambda => DeserializeLambda(ref reader, options),
            DiscriminatorListInit => DeserializeListInit(ref reader, options),
            DiscriminatorMember => DeserializeMember(ref reader, options),
            DiscriminatorMemberInit => DeserializeMemberInit(ref reader, options),
            DiscriminatorNew => DeserializeNew(ref reader, options),
            DiscriminatorParameter => DeserializeParameter(ref reader, options),
            DiscriminatorQuote => DeserializeQuote(ref reader, options),
            _ => throw new MessagePackSerializationException($"Unknown expression node discriminator '{discriminator}'.")
        };
    }

    /// <inheritdoc />
    public void Serialize(ref MessagePackWriter writer, ExpressionNodeBase? value, MessagePackSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNil();

            return;
        }

        writer.WriteArrayHeader(2);

        switch (value)
        {
            case BinaryExpressionNode binary:
                writer.Write(DiscriminatorBinary);
                SerializeBinary(ref writer, binary, options);
                break;
            case BlockExpressionNode block:
                writer.Write(DiscriminatorBlock);
                SerializeBlock(ref writer, block, options);
                break;
            case ConditionalExpressionNode conditional:
                writer.Write(DiscriminatorConditional);
                SerializeConditional(ref writer, conditional, options);
                break;
            case ConstantExpressionNode constant:
                writer.Write(DiscriminatorConstant);
                SerializeConstant(ref writer, constant, options);
                break;
            case InvocationExpressionNode invocation:
                writer.Write(DiscriminatorInvocation);
                SerializeInvocation(ref writer, invocation, options);
                break;
            case LambdaExpressionNode lambda:
                writer.Write(DiscriminatorLambda);
                SerializeLambda(ref writer, lambda, options);
                break;
            case ListInitExpressionNode listInit:
                writer.Write(DiscriminatorListInit);
                SerializeListInit(ref writer, listInit, options);
                break;
            case MemberExpressionNode member:
                writer.Write(DiscriminatorMember);
                SerializeMember(ref writer, member, options);
                break;
            case MemberInitExpressionNode memberInit:
                writer.Write(DiscriminatorMemberInit);
                SerializeMemberInit(ref writer, memberInit, options);
                break;
            case NewExpressionNode @new:
                writer.Write(DiscriminatorNew);
                SerializeNew(ref writer, @new, options);
                break;
            case ParameterExpressionNode parameter:
                writer.Write(DiscriminatorParameter);
                SerializeParameter(ref writer, parameter, options);
                break;
            case QuoteExpressionNode quote:
                writer.Write(DiscriminatorQuote);
                SerializeQuote(ref writer, quote, options);
                break;
            default:
                throw new MessagePackSerializationException($"Unsupported expression node type '{value.GetType().FullName}'.");
        }
    }

    private static void SerializeBinary(ref MessagePackWriter writer, BinaryExpressionNode node, MessagePackSerializerOptions options)
    {
        writer.WriteMapHeader(4);
        WriteNodeType(ref writer, node.NodeType);
        WriteType(ref writer, node.Type, options);
        writer.Write(KeyLeft);
        SerializeNode(ref writer, node.Left, options);
        writer.Write(KeyRight);
        SerializeNode(ref writer, node.Right, options);
    }

    private static BinaryExpressionNode DeserializeBinary(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var map = ReadMap(ref reader);
        var node = new BinaryExpressionNode(ReadNodeType(map), ReadClrType(map, options))
        {
            Left = ReadNode(map, KeyLeft, options),
            Right = ReadNode(map, KeyRight, options)
        };

        return node;
    }

    private static void SerializeBlock(ref MessagePackWriter writer, BlockExpressionNode node, MessagePackSerializerOptions options)
    {
        writer.WriteMapHeader(4);
        WriteNodeType(ref writer, node.NodeType);
        WriteType(ref writer, node.Type, options);
        writer.Write(KeyExpressions);
        SerializeNodeCollection(ref writer, node.Expressions, options);
        writer.Write(KeyVariables);
        SerializeParameterCollection(ref writer, node.Variables, options);
    }

    private static BlockExpressionNode DeserializeBlock(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var map = ReadMap(ref reader);
        var node = new BlockExpressionNode(ReadNodeType(map), ReadClrType(map, options))
        {
            Expressions = ReadNodeCollection(map, KeyExpressions, options),
            Variables = ReadParameterCollection(map, KeyVariables, options)
        };

        return node;
    }

    private static void SerializeConditional(ref MessagePackWriter writer, ConditionalExpressionNode node, MessagePackSerializerOptions options)
    {
        writer.WriteMapHeader(5);
        WriteNodeType(ref writer, node.NodeType);
        WriteType(ref writer, node.Type, options);
        writer.Write(KeyTest);
        SerializeNode(ref writer, node.Test, options);
        writer.Write(KeyIfTrue);
        SerializeNode(ref writer, node.IfTrue, options);
        writer.Write(KeyIfFalse);
        SerializeNode(ref writer, node.IfFalse, options);
    }

    private static ConditionalExpressionNode DeserializeConditional(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var map = ReadMap(ref reader);
        var node = new ConditionalExpressionNode(ReadNodeType(map), ReadClrType(map, options))
        {
            Test = ReadNode(map, KeyTest, options),
            IfTrue = ReadNode(map, KeyIfTrue, options),
            IfFalse = ReadNode(map, KeyIfFalse, options)
        };

        return node;
    }

    private static void SerializeConstant(ref MessagePackWriter writer, ConstantExpressionNode node, MessagePackSerializerOptions options)
    {
        writer.WriteMapHeader(3);
        WriteNodeType(ref writer, node.NodeType);
        WriteType(ref writer, node.Type, options);
        writer.Write(KeyValue);
        if (node.Value is null)
            writer.WriteNil();
        else
            MessagePackSerializer.Serialize(node.Type, ref writer, node.Value, options);
    }

    private static ConstantExpressionNode DeserializeConstant(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var map = ReadMap(ref reader);
        var type = ReadClrType(map, options);
        var valueReader = GetReader(map, KeyValue);
        var value = valueReader.TryReadNil() ? null : MessagePackSerializer.Deserialize(type, ref valueReader, options);

        return new ConstantExpressionNode(ReadNodeType(map), type)
        {
            Value = value
        };
    }

    private static void SerializeInvocation(ref MessagePackWriter writer, InvocationExpressionNode node, MessagePackSerializerOptions options)
    {
        writer.WriteMapHeader(4);
        WriteNodeType(ref writer, node.NodeType);
        WriteType(ref writer, node.Type, options);
        writer.Write(KeyExpression);
        SerializeNode(ref writer, node.Expression, options);
        writer.Write(KeyArguments);
        SerializeNodeCollection(ref writer, node.Arguments, options);
    }

    private static InvocationExpressionNode DeserializeInvocation(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var map = ReadMap(ref reader);
        var node = new InvocationExpressionNode(ReadNodeType(map), ReadClrType(map, options))
        {
            Expression = ReadNode(map, KeyExpression, options),
            Arguments = ReadNodeCollection(map, KeyArguments, options)
        };

        return node;
    }

    private static void SerializeLambda(ref MessagePackWriter writer, LambdaExpressionNode node, MessagePackSerializerOptions options)
    {
        writer.WriteMapHeader(4);
        WriteNodeType(ref writer, node.NodeType);
        WriteType(ref writer, node.Type, options);
        writer.Write(KeyBody);
        SerializeNode(ref writer, node.Body, options);
        writer.Write(KeyParameters);
        SerializeParameterCollection(ref writer, node.Parameters, options);
    }

    private static LambdaExpressionNode DeserializeLambda(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var map = ReadMap(ref reader);
        var node = new LambdaExpressionNode(ReadNodeType(map), ReadClrType(map, options))
        {
            Body = ReadNode(map, KeyBody, options),
            Parameters = ReadParameterCollection(map, KeyParameters, options)
        };

        return node;
    }

    private static void SerializeListInit(ref MessagePackWriter writer, ListInitExpressionNode node, MessagePackSerializerOptions options)
    {
        writer.WriteMapHeader(4);
        WriteNodeType(ref writer, node.NodeType);
        WriteType(ref writer, node.Type, options);
        writer.Write(KeyNewExpression);
        SerializeNode(ref writer, node.NewExpression, options);
        writer.Write(KeyInitializers);
        SerializeElementInitCollection(ref writer, node.Initializers, options);
    }

    private static ListInitExpressionNode DeserializeListInit(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var map = ReadMap(ref reader);
        var node = new ListInitExpressionNode(ReadNodeType(map), ReadClrType(map, options))
        {
            NewExpression = (NewExpressionNode) ReadNode(map, KeyNewExpression, options),
            Initializers = ReadElementInitCollection(map, KeyInitializers, options)
        };

        return node;
    }

    private static void SerializeMember(ref MessagePackWriter writer, MemberExpressionNode node, MessagePackSerializerOptions options)
    {
        writer.WriteMapHeader(4);
        WriteNodeType(ref writer, node.NodeType);
        WriteType(ref writer, node.Type, options);
        writer.Write(KeyExpression);
        SerializeNode(ref writer, node.Expression, options);
        writer.Write(KeyMemberName);
        writer.Write(node.MemberName);
    }

    private static MemberExpressionNode DeserializeMember(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var map = ReadMap(ref reader);
        var node = new MemberExpressionNode(ReadNodeType(map), ReadClrType(map, options))
        {
            Expression = ReadNode(map, KeyExpression, options),
            MemberName = ReadString(map, KeyMemberName)
        };

        return node;
    }

    private static void SerializeMemberInit(ref MessagePackWriter writer, MemberInitExpressionNode node, MessagePackSerializerOptions options)
    {
        writer.WriteMapHeader(4);
        WriteNodeType(ref writer, node.NodeType);
        WriteType(ref writer, node.Type, options);
        writer.Write(KeyNewExpression);
        SerializeNode(ref writer, node.NewExpression, options);
        writer.Write(KeyBindings);
        SerializeMemberAssignmentCollection(ref writer, node.Bindings, options);
    }

    private static MemberInitExpressionNode DeserializeMemberInit(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var map = ReadMap(ref reader);
        var node = new MemberInitExpressionNode(ReadNodeType(map), ReadClrType(map, options))
        {
            NewExpression = (NewExpressionNode) ReadNode(map, KeyNewExpression, options),
            Bindings = ReadMemberAssignmentCollection(map, KeyBindings, options)
        };

        return node;
    }

    private static void SerializeNew(ref MessagePackWriter writer, NewExpressionNode node, MessagePackSerializerOptions options)
    {
        writer.WriteMapHeader(5);
        WriteNodeType(ref writer, node.NodeType);
        WriteType(ref writer, node.Type, options);
        writer.Write(KeyArguments);
        SerializeNodeCollection(ref writer, node.Arguments, options);
        writer.Write(KeyConstructorParameterTypes);
        SerializeTypeCollection(ref writer, node.ConstructorParameterTypes, options);
        writer.Write(KeyMemberNames);
        SerializeStringCollection(ref writer, node.MemberNames);
    }

    private static NewExpressionNode DeserializeNew(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var map = ReadMap(ref reader);
        var node = new NewExpressionNode(ReadNodeType(map), ReadClrType(map, options))
        {
            Arguments = ReadNodeCollection(map, KeyArguments, options),
            ConstructorParameterTypes = ReadTypeCollection(map, KeyConstructorParameterTypes, options),
            MemberNames = ReadStringCollection(map, KeyMemberNames)
        };

        return node;
    }

    private static void SerializeParameter(ref MessagePackWriter writer, ParameterExpressionNode node, MessagePackSerializerOptions options)
    {
        writer.WriteMapHeader(3);
        WriteNodeType(ref writer, node.NodeType);
        WriteType(ref writer, node.Type, options);
        writer.Write(KeyName);
        writer.Write(node.Name);
    }

    private static ParameterExpressionNode DeserializeParameter(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var map = ReadMap(ref reader);

        return new ParameterExpressionNode(ReadNodeType(map), ReadClrType(map, options))
        {
            Name = map.TryGetValue(KeyName, out var nameBytes) ? ReadOptionalString(nameBytes) : null
        };
    }

    private static void SerializeQuote(ref MessagePackWriter writer, QuoteExpressionNode node, MessagePackSerializerOptions options)
    {
        writer.WriteMapHeader(3);
        WriteNodeType(ref writer, node.NodeType);
        WriteType(ref writer, node.Type, options);
        writer.Write(KeyOperand);
        SerializeNode(ref writer, node.Operand, options);
    }

    private static QuoteExpressionNode DeserializeQuote(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var map = ReadMap(ref reader);
        var node = new QuoteExpressionNode(ReadNodeType(map), ReadClrType(map, options))
        {
            Operand = ReadNode(map, KeyOperand, options)
        };

        return node;
    }

    private static void SerializeElementInitCollection(ref MessagePackWriter writer, IReadOnlyCollection<ElementInitNode> initializers, MessagePackSerializerOptions options)
    {
        writer.WriteArrayHeader(initializers.Count);
        foreach (var initializer in initializers)
        {
            writer.WriteMapHeader(2);
            writer.Write(KeyAddMethodName);
            writer.Write(initializer.AddMethodName);
            writer.Write(KeyArguments);
            SerializeNodeCollection(ref writer, initializer.Arguments, options);
        }
    }

    private static IReadOnlyCollection<ElementInitNode> ReadElementInitCollection(Dictionary<String, ReadOnlySequence<Byte>> map, String key, MessagePackSerializerOptions options)
    {
        var reader = GetReader(map, key);
        var count = reader.ReadArrayHeader();
        var result = new ElementInitNode[count];

        for (var i = 0; i < count; i++)
        {
            var itemMap = ReadMap(ref reader);

            result[i] = new ElementInitNode
            {
                AddMethodName = ReadString(itemMap, KeyAddMethodName),
                Arguments = ReadNodeCollection(itemMap, KeyArguments, options)
            };
        }

        return result;
    }

    private static void SerializeMemberAssignmentCollection(ref MessagePackWriter writer, IReadOnlyCollection<MemberAssignmentNode> bindings, MessagePackSerializerOptions options)
    {
        writer.WriteArrayHeader(bindings.Count);
        foreach (var binding in bindings)
        {
            writer.WriteMapHeader(2);
            writer.Write(KeyMemberName);
            writer.Write(binding.MemberName);
            writer.Write(KeyExpression);
            SerializeNode(ref writer, binding.Expression, options);
        }
    }

    private static IReadOnlyCollection<MemberAssignmentNode> ReadMemberAssignmentCollection(Dictionary<String, ReadOnlySequence<Byte>> map, String key, MessagePackSerializerOptions options)
    {
        var reader = GetReader(map, key);
        var count = reader.ReadArrayHeader();
        var result = new MemberAssignmentNode[count];

        for (var i = 0; i < count; i++)
        {
            var itemMap = ReadMap(ref reader);

            result[i] = new MemberAssignmentNode
            {
                MemberName = ReadString(itemMap, KeyMemberName),
                Expression = ReadNode(itemMap, KeyExpression, options)
            };
        }

        return result;
    }

    private static void SerializeNode(ref MessagePackWriter writer, ExpressionNodeBase node, MessagePackSerializerOptions options)
    {
        options.Resolver.GetFormatterWithVerify<ExpressionNodeBase?>().Serialize(ref writer, node, options);
    }

    private static void SerializeNodeCollection(ref MessagePackWriter writer, IReadOnlyCollection<ExpressionNodeBase> nodes, MessagePackSerializerOptions options)
    {
        writer.WriteArrayHeader(nodes.Count);
        foreach (var node in nodes)
            SerializeNode(ref writer, node, options);
    }

    private static void SerializeParameterCollection(ref MessagePackWriter writer, IReadOnlyCollection<ParameterExpressionNode> parameters, MessagePackSerializerOptions options)
    {
        writer.WriteArrayHeader(parameters.Count);
        foreach (var parameter in parameters)
            SerializeNode(ref writer, parameter, options);
    }

    private static void SerializeTypeCollection(ref MessagePackWriter writer, IReadOnlyCollection<Type> types, MessagePackSerializerOptions options)
    {
        writer.WriteArrayHeader(types.Count);
        var formatter = options.Resolver.GetFormatterWithVerify<Type?>();

        foreach (var type in types)
            formatter.Serialize(ref writer, type, options);
    }

    private static void SerializeStringCollection(ref MessagePackWriter writer, IReadOnlyCollection<String> values)
    {
        writer.WriteArrayHeader(values.Count);
        foreach (var value in values)
            writer.Write(value);
    }

    private static void WriteNodeType(ref MessagePackWriter writer, ExpressionType nodeType)
    {
        writer.Write(KeyNodeType);
        writer.Write((Int32) nodeType);
    }

    private static void WriteType(ref MessagePackWriter writer, Type type, MessagePackSerializerOptions options)
    {
        writer.Write(KeyType);
        options.Resolver.GetFormatterWithVerify<Type?>().Serialize(ref writer, type, options);
    }

    private static Dictionary<String, ReadOnlySequence<Byte>> ReadMap(ref MessagePackReader reader)
    {
        var count = reader.ReadMapHeader();
        var map = new Dictionary<String, ReadOnlySequence<Byte>>(count, StringComparer.Ordinal);

        for (var i = 0; i < count; i++)
        {
            var key = reader.ReadString() ?? String.Empty;

            map[key] = reader.ReadRaw();
        }

        return map;
    }

    private static ExpressionType ReadNodeType(Dictionary<String, ReadOnlySequence<Byte>> map)
    {
        var reader = GetReader(map, KeyNodeType);

        return (ExpressionType) reader.ReadInt32();
    }

    private static Type ReadClrType(Dictionary<String, ReadOnlySequence<Byte>> map, MessagePackSerializerOptions options)
    {
        var reader = GetReader(map, KeyType);
        var type = options.Resolver.GetFormatterWithVerify<Type?>().Deserialize(ref reader, options);

        if (type is null)
            throw new MessagePackSerializationException($"Expression node payload '{KeyType}' must not be null.");

        return type;
    }

    private static ExpressionNodeBase ReadNode(Dictionary<String, ReadOnlySequence<Byte>> map, String key, MessagePackSerializerOptions options)
    {
        var reader = GetReader(map, key);
        var node = options.Resolver.GetFormatterWithVerify<ExpressionNodeBase?>().Deserialize(ref reader, options);

        if (node is null)
            throw new MessagePackSerializationException($"Expression node payload '{key}' must not be null.");

        return node;
    }

    private static IReadOnlyCollection<ExpressionNodeBase> ReadNodeCollection(Dictionary<String, ReadOnlySequence<Byte>> map, String key, MessagePackSerializerOptions options)
    {
        var reader = GetReader(map, key);
        var count = reader.ReadArrayHeader();
        var result = new ExpressionNodeBase[count];
        var formatter = options.Resolver.GetFormatterWithVerify<ExpressionNodeBase?>();

        for (var i = 0; i < count; i++)
        {
            var node = formatter.Deserialize(ref reader, options);

            if (node is null)
                throw new MessagePackSerializationException($"Expression node collection '{key}' must not contain null entries.");

            result[i] = node;
        }

        return result;
    }

    private static IReadOnlyCollection<ParameterExpressionNode> ReadParameterCollection(Dictionary<String, ReadOnlySequence<Byte>> map, String key, MessagePackSerializerOptions options)
    {
        var reader = GetReader(map, key);
        var count = reader.ReadArrayHeader();
        var result = new ParameterExpressionNode[count];
        var formatter = options.Resolver.GetFormatterWithVerify<ExpressionNodeBase?>();

        for (var i = 0; i < count; i++)
        {
            var node = formatter.Deserialize(ref reader, options);

            if (node is not ParameterExpressionNode parameter)
                throw new MessagePackSerializationException($"Expression node collection '{key}' must contain parameter nodes.");

            result[i] = parameter;
        }

        return result;
    }

    private static IReadOnlyCollection<Type> ReadTypeCollection(Dictionary<String, ReadOnlySequence<Byte>> map, String key, MessagePackSerializerOptions options)
    {
        var reader = GetReader(map, key);
        var count = reader.ReadArrayHeader();
        var result = new Type[count];
        var formatter = options.Resolver.GetFormatterWithVerify<Type?>();

        for (var i = 0; i < count; i++)
        {
            var type = formatter.Deserialize(ref reader, options);

            if (type is null)
                throw new MessagePackSerializationException($"Type collection '{key}' must not contain null entries.");

            result[i] = type;
        }

        return result;
    }

    private static IReadOnlyCollection<String> ReadStringCollection(Dictionary<String, ReadOnlySequence<Byte>> map, String key)
    {
        var reader = GetReader(map, key);
        var count = reader.ReadArrayHeader();
        var result = new String[count];

        for (var i = 0; i < count; i++)
            result[i] = reader.ReadString() ?? String.Empty;

        return result;
    }

    private static String ReadString(Dictionary<String, ReadOnlySequence<Byte>> map, String key)
    {
        var reader = GetReader(map, key);

        return reader.ReadString() ?? String.Empty;
    }

    private static String? ReadOptionalString(ReadOnlySequence<Byte> bytes)
    {
        var reader = new MessagePackReader(bytes);

        return reader.TryReadNil() ? null : reader.ReadString();
    }

    private static MessagePackReader GetReader(Dictionary<String, ReadOnlySequence<Byte>> map, String key)
    {
        if (! map.TryGetValue(key, out var bytes))
            throw new MessagePackSerializationException($"Expression node payload missing '{key}'.");

        return new MessagePackReader(bytes);
    }
}
