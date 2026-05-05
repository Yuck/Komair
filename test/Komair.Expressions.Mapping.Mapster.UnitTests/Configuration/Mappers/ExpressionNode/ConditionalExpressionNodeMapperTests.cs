using System.Linq.Expressions;
using NUnit.Framework;

namespace Komair.Expressions.Mapping.Mapster.UnitTests.Configuration.Mappers.ExpressionNode;

public class ConditionalExpressionNodeMapperTests
{
    [Test]
    public void ToExpression_WhenConditionalExpression_RoundTripsEvaluation()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Int32, Int32>>();
        Expression<Func<Int32, Int32>> expression = t => t > 0 ? 1 : -1;
        var node = mapper.ToExpressionNode(expression);
        var roundTripped = mapper.ToExpression(node);

        Assert.AreEqual(expression.Compile()(5), roundTripped.Compile()(5));
        Assert.AreEqual(expression.Compile()(-5), roundTripped.Compile()(-5));
    }
}
