using System.Linq.Expressions;
using Komair.Expressions.Mapping.Exceptions;
using NUnit.Framework;

namespace Komair.Expressions.Mapping.Mapster.UnitTests;

public class MapsterExpressionNodeMapperTests
{
    [Test]
    public void ToExpression_WhenRootIsNotLambda_ThrowsInvalidNodeRootException()
    {
        var mapper = new MapsterExpressionNodeMapper<Func<Boolean>>();
        var node = new ConstantExpressionNode(ExpressionType.Constant, typeof(Boolean)) { Value = true };

        var exception = Assert.Throws<InvalidNodeRootException>(() => mapper.ToExpression(node));

        Assert.AreEqual(typeof(ConstantExpressionNode), exception!.ActualNodeType);
    }
}
