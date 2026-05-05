using System.Linq.Expressions;
using Komair.Expressions.Extensions;
using NUnit.Framework;

namespace Komair.Expressions.UnitTests.Extensions;

public class ExpressionExtensionsTests
{
    [Test]
    public void GetParameterList_WhenBinaryExpressionNull_ThrowsArgumentNullException()
    {
        var binary = GetNullReference<BinaryExpression>();

        Assert.Throws<ArgumentNullException>(() => binary.GetParameterList());
    }

    [Test]
    public void GetParameterList_WhenExpressionNull_ThrowsArgumentNullException()
    {
        var expression = GetNullReference<Expression>();

        Assert.Throws<ArgumentNullException>(() => expression.GetParameterList());
    }

    [Test]
    public void GetParameterList_WhenInvocation_ReturnsDistinctParameters()
    {
        var parameter = Expression.Parameter(typeof(Int32), "t");
        var lambda = Expression.Lambda<Func<Int32, Int32>>(Expression.Add(parameter, Expression.Constant(1)), parameter);
        var expression = Expression.Invoke(lambda, parameter);

        var result = expression.GetParameterList();

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.AreSame(parameter, result.Single());
    }

    private static T GetNullReference<T>() where T : class => null!;
}
