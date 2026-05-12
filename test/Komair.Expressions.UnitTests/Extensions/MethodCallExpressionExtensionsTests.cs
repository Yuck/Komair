using System.Linq.Expressions;
using Komair.Expressions.Extensions;
using NUnit.Framework;

namespace Komair.Expressions.UnitTests.Extensions;

public class MethodCallExpressionExtensionsTests
{
    [Test]
    public void GetParameterList_WhenExpressionNull_ThrowsArgumentNullException()
    {
        var call = GetNullReference<MethodCallExpression>();

        Assert.Throws<ArgumentNullException>(() => call.GetParameterList());
    }

    [Test]
    public void GetParameterList_WhenInstanceCallWithNoArguments_ReturnsInstanceParameter()
    {
        var parameter = Expression.Parameter(typeof(Int32), "n");
        var method = typeof(Int32).GetMethod(nameof(Int32.ToString), Type.EmptyTypes)!;
        var call = Expression.Call(parameter, method);

        var result = call.GetParameterList();

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.AreSame(parameter, result.Single());
    }

    [Test]
    public void GetParameterList_WhenStaticCallPassesSameParameterTwice_ReturnsSingleParameter()
    {
        var parameter = Expression.Parameter(typeof(String), "s");
        var method = typeof(String).GetMethod(nameof(String.Concat), [typeof(String), typeof(String)])!;
        var call = Expression.Call(method, parameter, parameter);

        var result = call.GetParameterList();

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.AreSame(parameter, result.Single());
    }

    [Test]
    public void GetParameterList_WhenStaticCall_ReturnsAllArgumentParameters()
    {
        var left = Expression.Parameter(typeof(Int32), "a");
        var right = Expression.Parameter(typeof(Int32), "b");
        var method = typeof(Math).GetMethod(nameof(Math.Max), [typeof(Int32), typeof(Int32)])!;
        var call = Expression.Call(method, left, right);

        var result = call.GetParameterList();

        Assert.That(result, Is.EquivalentTo(new[] { left, right }));
    }

    private static T GetNullReference<T>() where T : class => null!;
}
