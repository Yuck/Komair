using NUnit.Framework;

namespace Komair.Specifications.UnitTests.Abstract;

public class BinarySpecificationBaseTests
{
    [Test]
    public void And_WhenBothSatisfied_IsSatisfiedByReturnsTrue()
    {
        var left = new SpecificationBaseTests.IsShortStringSpecification();
        var right = new SpecificationBaseTests.ContainsOrtSpecification();
        var specification = left.And(right);
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void And_WhenLeftNotSatisfied_IsSatisfiedByReturnsFalse()
    {
        var left = new SpecificationBaseTests.IsShortStringSpecification();
        var right = new SpecificationBaseTests.ContainsLongSpecification();
        var specification = left.And(right);
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsFalse(result);
    }

    [Test]
    public void Or_WhenEitherSatisfied_IsSatisfiedByReturnsTrue()
    {
        var left = new SpecificationBaseTests.IsShortStringSpecification();
        var right = new SpecificationBaseTests.ContainsLongSpecification();
        var specification = left.Or(right);
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsTrue(result);
    }

    [Test]
    public void Or_WhenNeitherSatisfied_IsSatisfiedByReturnsFalse()
    {
        var left = new SpecificationBaseTests.IsShortStringSpecification();
        var right = new SpecificationBaseTests.ContainsOrtSpecification();
        var specification = left.Or(right);
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsFalse(result);
    }
}
