using System.Linq.Expressions;
using NUnit.Framework;

namespace Komair.Expressions.Mapping.Mapster.UnitTests.Configuration.Mappers.ExpressionNode;

public class QuoteExpressionNodeMapperTests
{
    [Test]
    public void ToExpression_WhenQuoteExpression_RoundTripsEvaluation()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Expression<Func<Int32>>>>();
        Expression<Func<Expression<Func<Int32>>>> expression = () => () => 6;
        var node = mapper.ToExpressionNode(expression);
        var roundTripped = mapper.ToExpression(node);
        var expected = expression.Compile()().Compile()();
        var actual = roundTripped.Compile()().Compile()();

        Assert.AreEqual(expected, actual);
    }
}
