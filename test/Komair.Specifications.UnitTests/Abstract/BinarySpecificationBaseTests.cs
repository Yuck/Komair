using NUnit.Framework;

namespace Komair.Specifications.UnitTests.Abstract;

public class BinarySpecificationBaseTests
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
    public void And_WhenLeftNotSatisfied_ReturnsFalse()
    {
        var left = new SpecificationBaseTests.IsShortStringSpecification();
        var right = new SpecificationBaseTests.ContainsLongSpecification();
        var specification = left.And(right);
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsFalse(result);
    }

    [Test]
    public void Or_WhenEitherSatisfied_ReturnsTrue()
    {
        var left = new SpecificationBaseTests.IsShortStringSpecification();
        var right = new SpecificationBaseTests.ContainsLongSpecification();
        var specification = left.Or(right);
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

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
}
