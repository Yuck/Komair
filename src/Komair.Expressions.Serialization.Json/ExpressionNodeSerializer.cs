using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Komair.Expressions.Abstract;
using Komair.Expressions.Serialization.Abstract.Interfaces;
using Komair.Expressions.Serialization.Json.Internal;

namespace Komair.Expressions.Serialization.Json;

public class ExpressionNodeSerializer<TExpressionNode>(JsonSerializerOptions? options = null) : IExpressionNodeSerializer<JsonObject, TExpressionNode> where TExpressionNode : ExpressionNodeBase
{
    private readonly JsonSerializerOptions _options = CreateOptions(options);

    public TExpressionNode Deserialize(JsonObject document)
    {
        var json = document.ToJsonString();

        var result = JsonSerializer.Deserialize<TExpressionNode>(json, _options);
        if (result is ExpressionNodeBase root)
            result = (TExpressionNode) MaterializeConstantValues(root);

        if (result is null)
            throw new JsonException($"Failed to deserialize {typeof(TExpressionNode).Name} from JSON.");

        return result;
    }

    public JsonObject Serialize(TExpressionNode node)
    {
        var json = JsonSerializer.SerializeToNode(node, _options);
        if (json is JsonObject value)
            return value;

        throw new JsonException("Expected JSON root to be an object.");
    }

    private static ExpressionNodeBase MaterializeConstantValues(ExpressionNodeBase node)
    {
        return node switch
        {
            ConstantExpressionNode constant => MaterializeConstant(constant),
            BinaryExpressionNode binary => MaterializeBinary(binary),
            LambdaExpressionNode lambda => MaterializeLambda(lambda),
            MemberExpressionNode member => MaterializeMember(member),
            ParameterExpressionNode parameter => parameter,
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
            return element.GetString() is { } name ? Enum.Parse(targetType, name) : null;

        return JsonSerializer.Deserialize(element.GetRawText(), targetType);
    }

    private static JsonSerializerOptions CreateOptions(JsonSerializerOptions? options)
    {
        var result = options is null ? new JsonSerializerOptions(JsonSerializerDefaults.General) : new JsonSerializerOptions(options);

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
                        t.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(ConstantExpressionNode), "Constant"));
                        t.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(LambdaExpressionNode), "Lambda"));
                        t.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(MemberExpressionNode), "Member"));
                        t.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(ParameterExpressionNode), "Parameter"));
                    }
                }
            }
        };

        return result;
    }
}
