using System.Linq.Expressions;
using Komair.Expressions.Abstract;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.Expression.Abstract;
using Mapster;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.Expression;

internal class ExpressionMapper<T>(TypeAdapterConfig configuration) : ExpressionMapperBase<Expression<T>, ExpressionNodeBase>(configuration)
{
    public override ExpressionNodeBase Map(Expression<T> source)
    {
        return source switch
        {
            LambdaExpression lambdaExpression => new LambdaExpressionMapper(Configuration).Map(lambdaExpression),
            _ => throw new NotSupportedException()
        };
    }
}
