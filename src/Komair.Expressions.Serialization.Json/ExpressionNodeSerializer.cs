using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Komair.Expressions.Abstract;
using Komair.Expressions.Serialization;
using Komair.Expressions.Serialization.Abstract.Interfaces;
using Komair.Expressions.Serialization.Exceptions;
using Komair.Expressions.Serialization.Json.Internal;

namespace Komair.Expressions.Serialization.Json;

/// <summary>
/// Serializes and deserializes <typeparamref name="TExpressionNode"/> instances using <see cref="System.Text.Json"/>.
/// </summary>
/// <typeparam name="TExpressionNode">The concrete expression node root type.</typeparam>
/// <param name="options">Optional JSON serializer options; defaults are used when <see langword="null"/>.</param>
public class ExpressionNodeSerializer<TExpressionNode>(JsonSerializerOptions? options = null) : IExpressionNodeSerializer<JsonObject, TExpressionNode> where TExpressionNode : ExpressionNodeBase
{
    private readonly JsonSerializerOptions _options = CreateOptions(options);

    /// <inheritdoc />
    public TExpressionNode Deserialize(JsonObject document)
    {
        var nodeDocument = UnwrapNodeDocument(document);

        return DeserializeNode(nodeDocument);
    }

    /// <inheritdoc />
    public JsonObject Serialize(TExpressionNode node)
    {
        var nodeDocument = SerializeNode(node);

        return new JsonObject
        {
            [ExpressionSerializationWireFormat.SchemaPropertyName] = ExpressionSerializationWireFormat.CurrentSchemaVersion,
            [ExpressionSerializationWireFormat.NodePropertyName] = nodeDocument
        };
    }

    private TExpressionNode DeserializeNode(JsonObject nodeDocument)
    {
        var json = nodeDocument.ToJsonString();

        TExpressionNode? result;
        try
        {
            result = JsonSerializer.Deserialize<TExpressionNode>(json, _options);
        }
        catch (Exception exception) when (exception is JsonException or NotSupportedException)
        {
            throw new ExpressionSerializationException($"Failed to deserialize {typeof(TExpressionNode).Name} from JSON.", exception);
        }

        if (result is ExpressionNodeBase root)
            result = (TExpressionNode) MaterializeConstantValues(root);

        if (result is null)
            throw new ExpressionSerializationException($"Failed to deserialize {typeof(TExpressionNode).Name} from JSON.");

        return result;
    }

    private JsonObject SerializeNode(TExpressionNode node)
    {
        var json = JsonSerializer.SerializeToNode(node, _options);
        if (json is JsonObject value)
            return value;

        throw new ExpressionSerializationException("Expected JSON root to be an object.");
    }

    private static JsonObject UnwrapNodeDocument(JsonObject document)
    {
        if (! document.ContainsKey(ExpressionSerializationWireFormat.SchemaPropertyName))
            return document;

        if (document[ExpressionSerializationWireFormat.SchemaPropertyName] is not JsonValue schemaValue)
            throw new ExpressionSerializationException($"Property '{ExpressionSerializationWireFormat.SchemaPropertyName}' must be a JSON number.");

        if (! schemaValue.TryGetValue(out Int32 schemaVersion))
            throw new ExpressionSerializationException($"Property '{ExpressionSerializationWireFormat.SchemaPropertyName}' must be a JSON number.");

        if (schemaVersion > ExpressionSerializationWireFormat.CurrentSchemaVersion)
            throw new ExpressionSerializationException($"Unsupported expression serialization schema version {schemaVersion}; maximum supported version is {ExpressionSerializationWireFormat.CurrentSchemaVersion}.");

        return schemaVersion switch
        {
            0 => GetSchemaZeroNode(document),
            1 => GetEnvelopeNode(document),
            _ => throw new ExpressionSerializationException($"Unsupported expression serialization schema version {schemaVersion}; migrate stored payloads or use a serializer that supports that schema.")
        };
    }

    private static JsonObject GetEnvelopeNode(JsonObject document)
    {
        if (document[ExpressionSerializationWireFormat.NodePropertyName] is not JsonObject nodeDocument)
            throw new ExpressionSerializationException($"Property '{ExpressionSerializationWireFormat.NodePropertyName}' must be a JSON object.");

        return nodeDocument;
    }

    private static JsonObject GetSchemaZeroNode(JsonObject document)
    {
        var nodeDocument = document.DeepClone().AsObject();

        nodeDocument.Remove(ExpressionSerializationWireFormat.SchemaPropertyName);

        return nodeDocument;
    }

    private static ExpressionNodeBase MaterializeConstantValues(ExpressionNodeBase node)
    {
        return node switch
        {
            BinaryExpressionNode binary => MaterializeBinary(binary),
            BlockExpressionNode block => MaterializeBlock(block),
            ConditionalExpressionNode conditional => MaterializeConditional(conditional),
            ConstantExpressionNode constant => MaterializeConstant(constant),
            InvocationExpressionNode invocation => MaterializeInvocation(invocation),
            LambdaExpressionNode lambda => MaterializeLambda(lambda),
            ListInitExpressionNode listInit => MaterializeListInit(listInit),
            MemberExpressionNode member => MaterializeMember(member),
            MemberInitExpressionNode memberInit => MaterializeMemberInit(memberInit),
            NewExpressionNode @new => MaterializeNew(@new),
            ParameterExpressionNode parameter => parameter,
            QuoteExpressionNode quote => MaterializeQuote(quote),
            _ => node
        };
    }

    private static ConstantExpressionNode MaterializeConstant(ConstantExpressionNode constant)
    {
        if (constant.Value is JsonElement element)
            constant.Value = ConvertJsonElement(element, constant.Type);

        return constant;
    }

    private static BinaryExpressionNode MaterializeBinary(BinaryExpressionNode binary)
    {
        binary.Left = MaterializeConstantValues(binary.Left);
        binary.Right = MaterializeConstantValues(binary.Right);

        return binary;
    }

    private static BlockExpressionNode MaterializeBlock(BlockExpressionNode block)
    {
        block.Expressions = [.. block.Expressions.Select(MaterializeConstantValues)];
        block.Variables = [.. block.Variables.Select(t => (ParameterExpressionNode) MaterializeConstantValues(t))];

        return block;
    }

    private static ConditionalExpressionNode MaterializeConditional(ConditionalExpressionNode conditional)
    {
        conditional.Test = MaterializeConstantValues(conditional.Test);
        conditional.IfTrue = MaterializeConstantValues(conditional.IfTrue);
        conditional.IfFalse = MaterializeConstantValues(conditional.IfFalse);

        return conditional;
    }

    private static InvocationExpressionNode MaterializeInvocation(InvocationExpressionNode invocation)
    {
        invocation.Expression = MaterializeConstantValues(invocation.Expression);
        invocation.Arguments = [.. invocation.Arguments.Select(MaterializeConstantValues)];

        return invocation;
    }

    private static LambdaExpressionNode MaterializeLambda(LambdaExpressionNode lambda)
    {
        lambda.Body = MaterializeConstantValues(lambda.Body);
        lambda.Parameters = [.. lambda.Parameters.Select(t => (ParameterExpressionNode) MaterializeConstantValues(t))];

        return lambda;
    }

    private static MemberExpressionNode MaterializeMember(MemberExpressionNode member)
    {
        member.Expression = MaterializeConstantValues(member.Expression);

        return member;
    }

    private static ListInitExpressionNode MaterializeListInit(ListInitExpressionNode listInit)
    {
        listInit.NewExpression = MaterializeNew(listInit.NewExpression);
        listInit.Initializers = [.. listInit.Initializers.Select(MaterializeElementInit)];

        return listInit;
    }

    private static MemberInitExpressionNode MaterializeMemberInit(MemberInitExpressionNode memberInit)
    {
        memberInit.NewExpression = MaterializeNew(memberInit.NewExpression);
        memberInit.Bindings = [.. memberInit.Bindings.Select(MaterializeMemberAssignment)];

        return memberInit;
    }

    private static NewExpressionNode MaterializeNew(NewExpressionNode @new)
    {
        @new.Arguments = [.. @new.Arguments.Select(MaterializeConstantValues)];

        return @new;
    }

    private static QuoteExpressionNode MaterializeQuote(QuoteExpressionNode quote)
    {
        quote.Operand = MaterializeConstantValues(quote.Operand);

        return quote;
    }

    private static ElementInitNode MaterializeElementInit(ElementInitNode initializer)
    {
        initializer.Arguments = [.. initializer.Arguments.Select(MaterializeConstantValues)];

        return initializer;
    }

    private static MemberAssignmentNode MaterializeMemberAssignment(MemberAssignmentNode assignment)
    {
        assignment.Expression = MaterializeConstantValues(assignment.Expression);

        return assignment;
    }

    private static Object? ConvertJsonElement(JsonElement element, Type targetType)
    {
        if (element.ValueKind == JsonValueKind.Null)
            return null;

        if (targetType == typeof(String))
            return element.GetString();
        if (targetType == typeof(Boolean))
            return element.GetBoolean();
        if (targetType == typeof(Int32))
            return element.GetInt32();
        if (targetType == typeof(Int64))
            return element.GetInt64();
        if (targetType == typeof(Double))
            return element.GetDouble();
        if (targetType == typeof(Single))
            return (Single) element.GetDouble();
        if (targetType.IsEnum)
        {
            if (element.ValueKind == JsonValueKind.Number)
                return Enum.ToObject(targetType, element.GetInt32());

            var name = element.GetString();

            return name is not null ? Enum.Parse(targetType, name) : null;
        }

        return JsonSerializer.Deserialize(element.GetRawText(), targetType);
    }

    private static JsonSerializerOptions CreateOptions(JsonSerializerOptions? options)
    {
        var result = options is not null ? new JsonSerializerOptions(options) : new JsonSerializerOptions(JsonSerializerDefaults.General);

        result.Converters.Add(new TypeJsonConverter());

        result.TypeInfoResolver = new DefaultJsonTypeInfoResolver
        {
            Modifiers =
            {
                t =>
                {
                    if (t.Type == typeof(ExpressionNodeBase))
                    {
                        t.PolymorphismOptions = new JsonPolymorphismOptions
                        {
                            TypeDiscriminatorPropertyName = "$type",
                            UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization
                        };

                        t.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(BinaryExpressionNode), "Binary"));
                        t.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(BlockExpressionNode), "Block"));
                        t.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(ConditionalExpressionNode), "Conditional"));
                        t.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(ConstantExpressionNode), "Constant"));
                        t.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(InvocationExpressionNode), "Invocation"));
                        t.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(LambdaExpressionNode), "Lambda"));
                        t.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(ListInitExpressionNode), "ListInit"));
                        t.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(MemberExpressionNode), "Member"));
                        t.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(MemberInitExpressionNode), "MemberInit"));
                        t.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(NewExpressionNode), "New"));
                        t.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(ParameterExpressionNode), "Parameter"));
                        t.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(QuoteExpressionNode), "Quote"));
                    }
                }
            }
        };

        return result;
    }
}
