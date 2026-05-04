using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Komair.Expressions.Abstract;
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
        var json = document.ToJsonString();

        var result = JsonSerializer.Deserialize<TExpressionNode>(json, _options);
        if (result is ExpressionNodeBase root)
            result = (TExpressionNode) MaterializeConstantValues(root);

        if (result is null)
            throw new ExpressionSerializationException($"Failed to deserialize {typeof(TExpressionNode).Name} from JSON.");

        return result;
    }

    /// <inheritdoc />
    public JsonObject Serialize(TExpressionNode node)
    {
        var json = JsonSerializer.SerializeToNode(node, _options);
        if (json is JsonObject value)
            return value;

        throw new ExpressionSerializationException("Expected JSON root to be an object.");
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
