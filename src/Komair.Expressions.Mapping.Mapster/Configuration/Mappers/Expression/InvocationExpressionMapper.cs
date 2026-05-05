using System.Linq.Expressions;
using Komair.Expressions.Abstract;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.Expression.Abstract;
using Mapster;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.Expression;

internal class InvocationExpressionMapper(TypeAdapterConfig configuration) : ExpressionMapperBase<InvocationExpression, InvocationExpressionNode>(configuration)
{
    public override InvocationExpressionNode Map(InvocationExpression source)
    {
        var nodeType = source.NodeType;
        var type = source.Type;
        var arguments = source.Arguments.Select(t => t.Adapt<ExpressionNodeBase>(Configuration)).ToArray();
        var result = new InvocationExpressionNode(nodeType, type)
        {
            Expression = source.Expression.Adapt<ExpressionNodeBase>(Configuration),
            Arguments = arguments
        };

        return result;
    }
}
