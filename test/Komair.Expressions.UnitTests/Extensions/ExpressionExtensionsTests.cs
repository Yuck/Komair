using System.Linq.Expressions;
using Komair.Expressions.Exceptions;
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
    public void GetParameterList_WhenInvocation_ThrowsUnsupportedExpressionException()
    {
        var func = Expression.Constant((Func<Int32>) (() => 0));
        var expression = Expression.Invoke(func);

        var exception = Assert.Throws<UnsupportedExpressionException>(() => expression.GetParameterList());

        Assert.AreEqual(ExpressionType.Invoke, exception!.NodeType);
    }

    private static T GetNullReference<T>() where T : class => null!;
}
