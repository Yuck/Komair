using Komair.Expressions.Abstract;
using Mapster;
using LinqExpression = System.Linq.Expressions.Expression;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode.Abstract;

internal abstract class ExpressionNodeMapperBase<TSource, TDestination>(TypeAdapterConfig configuration) where TSource : ExpressionNodeBase where TDestination : LinqExpression
{
    protected TypeAdapterConfig Configuration = configuration;

    public abstract TDestination Map(TSource source);
}
