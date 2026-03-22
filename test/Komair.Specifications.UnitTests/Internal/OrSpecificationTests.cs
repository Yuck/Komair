using Komair.Specifications.UnitTests.Abstract;
using NUnit.Framework;

namespace Komair.Specifications.UnitTests.Internal;

public class OrSpecificationTests
{
    [Test]
    public void Or_WhenBothSatisfied_ReturnsTrue()
    {
        var left = new SpecificationBaseTests.IsShortStringSpecification();
        var right = new SpecificationBaseTests.ContainsOrtSpecification();
        var specification = left.Or(right);
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void Or_WhenNeitherSatisfied_ReturnsFalse()
    {
        var left = new SpecificationBaseTests.IsShortStringSpecification();
        var right = new SpecificationBaseTests.ContainsOrtSpecification();
        var specification = left.Or(right);
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsFalse(result);
    }

    [Test]
    public void Or_WhenOnlyLeftSatisfied_ReturnsTrue()
    {
        var left = new SpecificationBaseTests.IsShortStringSpecification();
        var right = new SpecificationBaseTests.ContainsLongSpecification();
        var specification = left.Or(right);
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void Or_WhenOnlyRightSatisfied_ReturnsTrue()
    {
        var left = new SpecificationBaseTests.IsShortStringSpecification();
        var right = new SpecificationBaseTests.ContainsLongSpecification();
        var specification = left.Or(right);
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsTrue(result);
    }
}
