using System.Linq.Expressions;
using Komair.Expressions.Mapping.Exceptions;
using NUnit.Framework;

namespace Komair.Expressions.Mapping.UnitTests.Exceptions;

public class InvalidTreeRootExceptionTests
{
    [Test]
    public void Constructor_WhenExpressionNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new InvalidTreeRootException(null!));
    }

    [Test]
    public void Constructor_WhenRootIsNotLambda_SetsNodeTypeAndMessage()
    {
        var expression = Expression.Constant(42);

        var exception = new InvalidTreeRootException(expression);

        Assert.AreEqual(ExpressionType.Constant, exception.NodeType);
        Assert.That(exception.Message, Does.Contain(ExpressionType.Constant.ToString()));
        Assert.That(exception.Message, Does.Contain(ExpressionType.Lambda.ToString()));
    }
}
