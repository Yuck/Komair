using Komair.Specifications.UnitTests.Abstract;
using NUnit.Framework;

namespace Komair.Specifications.UnitTests.Internal;

public class OrSpecificationTests : TestBase
{
    [Test]
    public void LeftIsFalse_And_RightIsFalse_IsFalse()
    {
        var left = new IsShortStringSpecification();
        var right = new ContainsOrtSpecification();
        var specification = left.Or(right);
        var result = specification.IsSatisfiedBy(LongString);

        Assert.IsFalse(result);
    }

    [Test]
    public void LeftIsFalse_And_RightIsTrue_IsTrue()
    {
        var left = new IsShortStringSpecification();
        var right = new ContainsLongSpecification();
        var specification = left.Or(right);
        var result = specification.IsSatisfiedBy(LongString);

        Assert.IsTrue(result);
    }

    [Test]
    public void LeftIsTrue_And_RightIsFalse_IsTrue()
    {
        var left = new IsShortStringSpecification();
        var right = new ContainsLongSpecification();
        var specification = left.Or(right);
        var result = specification.IsSatisfiedBy(ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void LeftIsTrue_And_RightIsTrue_IsTrue()
    {
        var left = new IsShortStringSpecification();
        var right = new ContainsOrtSpecification();
        var specification = left.Or(right);
        var result = specification.IsSatisfiedBy(ShortString);

        Assert.IsTrue(result);
    }
}
