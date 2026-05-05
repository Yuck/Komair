using System.Linq.Expressions;
using NUnit.Framework;

namespace Komair.Expressions.Mapping.Mapster.UnitTests.Configuration.Mappers.ExpressionNode;

public class InvocationExpressionNodeMapperTests
{
    [Test]
    public void ToExpression_WhenInvocationExpression_RoundTripsEvaluation()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Int32>>();
        var innerLambda = Expression.Lambda<Func<Int32>>(Expression.Constant(6));
        var invoke = Expression.Invoke(innerLambda);
        var expression = Expression.Lambda<Func<Int32>>(invoke);
        var node = mapper.ToExpressionNode(expression);
        var roundTripped = mapper.ToExpression(node);

        Assert.AreEqual(expression.Compile()(), roundTripped.Compile()());
    }
}
