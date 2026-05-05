using System.Linq.Expressions;
using NUnit.Framework;

namespace Komair.Expressions.Mapping.Mapster.UnitTests.Configuration.Mappers.ExpressionNode;

public class MemberInitExpressionNodeMapperTests
{
    [Test]
    public void ToExpression_WhenMemberInitExpression_RoundTripsEvaluation()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Int32, TestModel>>();
        Expression<Func<Int32, TestModel>> expression = t => new TestModel { Value = t };
        var node = mapper.ToExpressionNode(expression);
        var roundTripped = mapper.ToExpression(node);

        Assert.AreEqual(expression.Compile()(5).Value, roundTripped.Compile()(5).Value);
    }

    public class TestModel
    {
        public Int32 Value { get; set; }
    }
}
