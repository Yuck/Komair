using System.Linq.Expressions;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode.Abstract;
using Mapster;
using LinqExpression = System.Linq.Expressions.Expression;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode;

internal class ConditionalExpressionNodeMapper(TypeAdapterConfig configuration) : ExpressionNodeMapperBase<ConditionalExpressionNode, ConditionalExpression>(configuration)
{
    public override ConditionalExpression Map(ConditionalExpressionNode source)
    {
        var test = source.Test.Adapt<LinqExpression>(Configuration);
        var ifTrue = source.IfTrue.Adapt<LinqExpression>(Configuration);
        var ifFalse = source.IfFalse.Adapt<LinqExpression>(Configuration);
        var result = LinqExpression.Condition(test, ifTrue, ifFalse);

        return result;
    }
}
