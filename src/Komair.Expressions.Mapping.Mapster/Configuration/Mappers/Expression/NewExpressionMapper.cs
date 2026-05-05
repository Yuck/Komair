using System.Linq.Expressions;
using Komair.Expressions.Abstract;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.Expression.Abstract;
using Mapster;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.Expression;

internal class NewExpressionMapper(TypeAdapterConfig configuration) : ExpressionMapperBase<NewExpression, NewExpressionNode>(configuration)
{
    public override NewExpressionNode Map(NewExpression source)
    {
        var nodeType = source.NodeType;
        var type = source.Type;
        var constructorParameterTypes = source.Constructor?.GetParameters().Select(t => t.ParameterType).ToArray() ?? [];
        var arguments = source.Arguments.Select(t => t.Adapt<ExpressionNodeBase>(Configuration)).ToArray();
        var result = new NewExpressionNode(nodeType, type)
        {
            Arguments = arguments,
            ConstructorParameterTypes = constructorParameterTypes,
            MemberNames = source.Members?.Select(t => t.Name).ToArray() ?? []
        };

        return result;
    }
}
