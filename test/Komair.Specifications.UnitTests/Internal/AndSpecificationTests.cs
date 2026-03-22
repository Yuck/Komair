using Komair.Specifications.UnitTests.Abstract;
using NUnit.Framework;

namespace Komair.Specifications.UnitTests.Internal;

public class AndSpecificationTests
{
    [Test]
    public void And_WhenBothSatisfied_ReturnsTrue()
    {
        var left = new SpecificationBaseTests.IsShortStringSpecification();
        var right = new SpecificationBaseTests.ContainsOrtSpecification();
        var specification = left.And(right);
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void And_WhenOnlyLeftSatisfied_ReturnsFalse()
    {
        var left = new SpecificationBaseTests.IsShortStringSpecification();
        var right = new SpecificationBaseTests.ContainsLongSpecification();
        var specification = left.And(right);
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void And_WhenOnlyRightSatisfied_ReturnsFalse()
    {
        var left = new SpecificationBaseTests.IsShortStringSpecification();
        var right = new SpecificationBaseTests.ContainsLongSpecification();
        var specification = left.And(right);
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsFalse(result);
    }
}
