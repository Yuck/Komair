using System.Linq.Expressions;
using Komair.Expressions.Abstract;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.Expression.Abstract;
using Mapster;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.Expression;

internal class ConditionalExpressionMapper(TypeAdapterConfig configuration) : ExpressionMapperBase<ConditionalExpression, ConditionalExpressionNode>(configuration)
{
    public override ConditionalExpressionNode Map(ConditionalExpression source)
    {
        var nodeType = source.NodeType;
        var type = source.Type;
        var result = new ConditionalExpressionNode(nodeType, type)
        {
            Test = source.Test.Adapt<ExpressionNodeBase>(Configuration),
            IfTrue = source.IfTrue.Adapt<ExpressionNodeBase>(Configuration),
            IfFalse = source.IfFalse.Adapt<ExpressionNodeBase>(Configuration)
        };

        return result;
    }
}
