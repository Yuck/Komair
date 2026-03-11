using System.Linq.Expressions;
using Komair.Expressions.Extensions;
using NUnit.Framework;

namespace Komair.Expressions.Serialization.UnitTests.Extensions;

public class ExpressionExtensionsTests
{
    [Test]
    public void Get_Parameter_List_Returns_Empty_For_Null_Expressions()
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
