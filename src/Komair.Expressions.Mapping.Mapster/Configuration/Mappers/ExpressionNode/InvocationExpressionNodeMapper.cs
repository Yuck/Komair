using System.Linq.Expressions;
using Komair.Expressions.Abstract;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode.Abstract;
using Mapster;
using LinqExpression = System.Linq.Expressions.Expression;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode;

internal class InvocationExpressionNodeMapper(TypeAdapterConfig configuration) : ExpressionNodeMapperBase<InvocationExpressionNode, InvocationExpression>(configuration)
{
    public override InvocationExpression Map(InvocationExpressionNode source)
    {
        var expression = MapNode(source.Expression);
        var arguments = source.Arguments.Select(MapNode).ToArray();
        var result = LinqExpression.Invoke(expression, arguments);

        return result;
    }

    private LinqExpression MapNode(ExpressionNodeBase source)
    {
        if (source is LambdaExpressionNode lambda)
        {
            var body = MapNode(lambda.Body);
            var parameters = lambda.Parameters.Select(t => (ParameterExpression) MapNode(t)).ToArray();

            return LinqExpression.Lambda(body, parameters);
        }

        return source.Adapt<LinqExpression>(Configuration);
    }
}
