using System.Linq.Expressions;
using NUnit.Framework;

namespace Komair.Expressions.Mapping.Mapster.UnitTests.Configuration.Mappers.ExpressionNode;

public class BlockExpressionNodeMapperTests
{
    [Test]
    public void ToExpression_WhenBlockExpression_RoundTripsEvaluation()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Int32>>();
        var block = Expression.Block(Expression.Constant(1), Expression.Constant(2));
        var expression = Expression.Lambda<Func<Int32>>(block);
        var node = mapper.ToExpressionNode(expression);
        var roundTripped = mapper.ToExpression(node);

        Assert.AreEqual(expression.Compile()(), roundTripped.Compile()());
    }
}
