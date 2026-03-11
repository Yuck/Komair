using System.Linq.Expressions;
using Komair.Expressions.Abstract;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode.Abstract;
using Mapster;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode;

internal class ExpressionNodeMapper<T>(TypeAdapterConfig configuration) : ExpressionNodeMapperBase<ExpressionNodeBase, Expression<T>>(configuration)
{
    public override Expression<T> Map(ExpressionNodeBase source)
    {
        return source switch
        {
            LambdaExpressionNode lambdaExpressionNode => new LambdaExpressionNodeMapper<T>(Configuration).Map(lambdaExpressionNode),
            _ => throw new NotSupportedException()
        };
    }
}
