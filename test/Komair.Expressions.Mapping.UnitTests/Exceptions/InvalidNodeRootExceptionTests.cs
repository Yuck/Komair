using System.Linq.Expressions;
using Komair.Expressions.Mapping.Exceptions;
using NUnit.Framework;

namespace Komair.Expressions.Mapping.UnitTests.Exceptions;

public class InvalidNodeRootExceptionTests
{
    [Test]
    public void Constructor_WhenNodeNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new InvalidNodeRootException(null!));
    }

    [Test]
    public void Constructor_WhenRootIsConstantNode_SetsActualNodeTypeAndMessage()
    {
        var node = new ConstantExpressionNode(ExpressionType.Constant, typeof(Boolean)) { Value = true };

        var exception = new InvalidNodeRootException(node);

        Assert.AreEqual(typeof(ConstantExpressionNode), exception.ActualNodeType);
        Assert.That(exception.Message, Does.Contain(nameof(ConstantExpressionNode)));
        Assert.That(exception.Message, Does.Contain(nameof(LambdaExpressionNode)));
    }
}
