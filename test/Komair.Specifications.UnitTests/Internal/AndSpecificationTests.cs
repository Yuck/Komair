using Komair.Specifications.UnitTests.Abstract;
using NUnit.Framework;

namespace Komair.Specifications.UnitTests.Internal;

public class AndSpecificationTests
{
    [Test]
    public void LeftIsFalse_And_RightIsTrue_IsFalse()
    {
        var left = new SpecificationBaseTests.IsShortStringSpecification();
        var right = new SpecificationBaseTests.ContainsLongSpecification();
        var specification = left.And(right);
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsFalse(result);
    }

    [Test]
    public void LeftIsTrue_And_RightIsFalse_IsFalse()
    {
        var left = new SpecificationBaseTests.IsShortStringSpecification();
        var right = new SpecificationBaseTests.ContainsLongSpecification();
        var specification = left.And(right);
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void LeftIsTrue_And_RightIsTrue_IsTrue()
    {
        var left = new SpecificationBaseTests.IsShortStringSpecification();
        var right = new SpecificationBaseTests.ContainsOrtSpecification();
        var specification = left.And(right);
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }
}
