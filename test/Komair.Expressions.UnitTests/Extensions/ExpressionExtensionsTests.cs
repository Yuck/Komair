using System.Linq.Expressions;
using Komair.Expressions.Extensions;
using NUnit.Framework;

namespace Komair.Expressions.UnitTests.Extensions;

public class ExpressionExtensionsTests
{
    [Test]
    public void GetParameterList_WhenExpressionNull_ReturnsEmpty()
    {
        var a = GetNullReference<Expression>();
        var b = GetNullReference<BinaryExpression>();

        var x = a.GetParameterList();
        var y = b.GetParameterList();

        Assert.IsNotNull(x);
        Assert.IsEmpty(x);

        Assert.IsNotNull(y);
        Assert.IsEmpty(y);
    }

    private static T GetNullReference<T>() where T : class => null!;
}
