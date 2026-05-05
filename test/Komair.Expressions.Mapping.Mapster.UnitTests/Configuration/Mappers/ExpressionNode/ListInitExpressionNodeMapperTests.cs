using System.Linq.Expressions;
using NUnit.Framework;

namespace Komair.Expressions.Mapping.Mapster.UnitTests.Configuration.Mappers.ExpressionNode;

public class ListInitExpressionNodeMapperTests
{
    [Test]
    public void ToExpression_WhenListInitExpression_RoundTripsEvaluation()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<List<Int32>>>();
        Expression<Func<List<Int32>>> expression = () => new List<Int32> { 5, 6 };
        var node = mapper.ToExpressionNode(expression);
        var roundTripped = mapper.ToExpression(node);
        var expected = expression.Compile()();
        var actual = roundTripped.Compile()();

        Assert.That(actual, Is.EqualTo(expected));
    }
}
