using System.Linq.Expressions;
using Komair.Expressions.Extensions;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode.Abstract;
using Mapster;
using LinqExpression = System.Linq.Expressions.Expression;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode;

internal class LambdaExpressionNodeMapper<T>(TypeAdapterConfig configuration) : ExpressionNodeMapperBase<LambdaExpressionNode, Expression<T>>(configuration)
{
    public override Expression<T> Map(LambdaExpressionNode source)
    {
        var body = source.Body.Adapt<LinqExpression>(Configuration);
        var parameters = body.GetParameterList();
        var result = LinqExpression.Lambda<T>(body, parameters);

        return result;
    }
}
