using Komair.Specifications.UnitTests.Abstract;
using NUnit.Framework;

namespace Komair.Specifications.UnitTests.Internal;

public class AndSpecificationTests : TestBase
{
    [Test]
    public void LeftIsFalse_And_RightIsTrue_IsFalse()
    {
        var left = new IsShortStringSpecification();
        var right = new ContainsLongSpecification();
        var specification = left.And(right);
        var result = specification.IsSatisfiedBy(LongString);

        Assert.IsFalse(result);
    }

    [Test]
    public void LeftIsTrue_And_RightIsFalse_IsFalse()
    {
        var left = new IsShortStringSpecification();
        var right = new ContainsLongSpecification();
        var specification = left.And(right);
        var result = specification.IsSatisfiedBy(ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void LeftIsTrue_And_RightIsTrue_IsTrue()
    {
        var left = new IsShortStringSpecification();
        var right = new ContainsOrtSpecification();
        var specification = left.And(right);
        var result = specification.IsSatisfiedBy(ShortString);

        Assert.IsTrue(result);
    }
}
