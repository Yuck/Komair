using System.Linq.Expressions;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode.Abstract;
using Mapster;
using LinqExpression = System.Linq.Expressions.Expression;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode;

internal class BlockExpressionNodeMapper(TypeAdapterConfig configuration) : ExpressionNodeMapperBase<BlockExpressionNode, BlockExpression>(configuration)
{
    public override BlockExpression Map(BlockExpressionNode source)
    {
        var variables = source.Variables.Adapt<IReadOnlyCollection<ParameterExpression>>(Configuration);
        var expressions = source.Expressions.Select(t => t.Adapt<LinqExpression>(Configuration)).ToArray();
        var result = LinqExpression.Block(source.Type, variables, expressions);

        return result;
    }
}
