using System.Linq.Expressions;
using NUnit.Framework;

namespace Komair.Expressions.Mapping.Mapster.UnitTests;

public class AdvancedExpressionNodeMapperTests
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

    [Test]
    public void ToExpression_WhenMemberInitExpression_RoundTripsEvaluation()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Int32, TestModel>>();
        Expression<Func<Int32, TestModel>> expression = t => new TestModel { Value = t };
        var node = mapper.ToExpressionNode(expression);
        var roundTripped = mapper.ToExpression(node);

        Assert.AreEqual(expression.Compile()(5).Value, roundTripped.Compile()(5).Value);
    }

    [Test]
    public void ToExpression_WhenNewExpression_RoundTripsEvaluation()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Int32, Tuple<Int32>>>();
        Expression<Func<Int32, Tuple<Int32>>> expression = t => new Tuple<Int32>(t);
        var node = mapper.ToExpressionNode(expression);
        var roundTripped = mapper.ToExpression(node);

        Assert.AreEqual(expression.Compile()(5).Item1, roundTripped.Compile()(5).Item1);
    }

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

    public class TestModel
    {
        public Int32 Value { get; set; }
    }
}
