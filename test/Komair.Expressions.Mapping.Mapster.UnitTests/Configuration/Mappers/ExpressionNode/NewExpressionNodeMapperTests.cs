using System.Linq.Expressions;
using NUnit.Framework;

namespace Komair.Expressions.Mapping.Mapster.UnitTests.Configuration.Mappers.ExpressionNode;

public class NewExpressionNodeMapperTests
{
    [Test]
    public void ToExpression_WhenNewExpression_RoundTripsEvaluation()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Int32, Tuple<Int32>>>();
        Expression<Func<Int32, Tuple<Int32>>> expression = t => new Tuple<Int32>(t);
        var node = mapper.ToExpressionNode(expression);
        var roundTripped = mapper.ToExpression(node);

        Assert.AreEqual(expression.Compile()(5).Item1, roundTripped.Compile()(5).Item1);
    }
}
