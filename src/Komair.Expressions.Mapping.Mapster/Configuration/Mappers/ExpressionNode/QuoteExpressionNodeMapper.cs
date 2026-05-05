using System.Linq.Expressions;
using Komair.Expressions.Abstract;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode.Abstract;
using Mapster;
using LinqExpression = System.Linq.Expressions.Expression;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode;

internal class QuoteExpressionNodeMapper(TypeAdapterConfig configuration) : ExpressionNodeMapperBase<QuoteExpressionNode, UnaryExpression>(configuration)
{
    public override UnaryExpression Map(QuoteExpressionNode source)
    {
        var operand = MapNode(source.Operand);
        var result = LinqExpression.Quote(operand);

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
