using System.Linq.Expressions;
using Komair.Expressions.Abstract;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.Expression.Abstract;
using Mapster;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.Expression;

internal class BlockExpressionMapper(TypeAdapterConfig configuration) : ExpressionMapperBase<BlockExpression, BlockExpressionNode>(configuration)
{
    public override BlockExpressionNode Map(BlockExpression source)
    {
        var nodeType = source.NodeType;
        var type = source.Type;
        var expressions = source.Expressions.Select(t => t.Adapt<ExpressionNodeBase>(Configuration)).ToArray();
        var result = new BlockExpressionNode(nodeType, type)
        {
            Expressions = expressions,
            Variables = source.Variables.Adapt<IReadOnlyCollection<ParameterExpressionNode>>(Configuration)
        };

        return result;
    }
}
