using System.Linq.Expressions;
using Komair.Expressions.Abstract;
using Komair.Expressions.Exceptions;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.Expression.Abstract;
using Mapster;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.Expression;

internal class QuoteExpressionMapper(TypeAdapterConfig configuration) : ExpressionMapperBase<UnaryExpression, QuoteExpressionNode>(configuration)
{
    public override QuoteExpressionNode Map(UnaryExpression source)
    {
        if (source.NodeType is not ExpressionType.Quote)
            throw new UnsupportedExpressionException(source.NodeType);

        var nodeType = source.NodeType;
        var type = source.Type;
        var result = new QuoteExpressionNode(nodeType, type)
        {
            Operand = source.Operand.Adapt<ExpressionNodeBase>(Configuration)
        };

        return result;
    }
}
