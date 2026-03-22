using Komair.Specifications.UnitTests.Abstract;
using NUnit.Framework;

namespace Komair.Specifications.UnitTests;

public class TrueSpecificationTests
{
    [Test]
    public void And_WhenAllCombinedSpecificationsSatisfied_ReturnsTrue()
    {
        var combined = TrueSpecification<String>.Identity.And(new SpecificationBaseTests.ContainsOrtSpecification(), new SpecificationBaseTests.IsShortStringSpecification());

        Assert.IsTrue(combined.IsSatisfiedBy(SpecificationBaseTests.ShortString));
    }

    [Test]
    public void And_WhenCombinedWithFalse_ReturnsFalse()
    {
        var specification = TrueSpecification<String>.Identity;
        var result = specification.And(FalseSpecification<String>.Identity).IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void And_WhenCombinedWithTrue_ReturnsTrue()
    {
        var specification = TrueSpecification<String>.Identity;
        var result = specification.And(TrueSpecification<String>.Identity).IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void And_WhenGivenTwoTrueSpecifications_ReturnsTrue()
    {
        var specification = TrueSpecification<String>.Identity.And(TrueSpecification<String>.Identity, TrueSpecification<String>.Identity);

        Assert.IsTrue(specification.IsSatisfiedBy(SpecificationBaseTests.ShortString));
    }

    [Test]
    public void And_WhenIncludesFalse_ReturnsFalse()
    {
        var specification = TrueSpecification<String>.Identity.And(TrueSpecification<String>.Identity, FalseSpecification<String>.Identity);

        Assert.IsFalse(specification.IsSatisfiedBy(SpecificationBaseTests.ShortString));
    }

    [Test]
    public void And_WhenNoAdditionalSpecifications_ReturnsTrue()
    {
        var specification = TrueSpecification<String>.Identity.And();

        Assert.IsTrue(specification.IsSatisfiedBy(SpecificationBaseTests.ShortString));
    }

    [Test]
    public void And_WhenOneCombinedSpecificationUnsatisfied_ReturnsFalse()
    {
        var combined = TrueSpecification<String>.Identity.And(new SpecificationBaseTests.ContainsOrtSpecification(), new SpecificationBaseTests.IsShortStringSpecification());

        Assert.IsFalse(combined.IsSatisfiedBy(SpecificationBaseTests.LongString));
    }

    [Test]
    public void IsSatisfiedBy_WhenCandidateIsLongString_ReturnsTrue()
    {
        var specification = TrueSpecification<String>.Identity;
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsTrue(result);
    }

    [Test]
    public void IsSatisfiedBy_WhenCandidateIsNull_ReturnsTrue()
    {
        var specification = TrueSpecification<String>.Identity;
        var result = specification.IsSatisfiedBy(null!);

        Assert.IsTrue(result);
    }

    [Test]
    public void IsSatisfiedBy_WhenCandidateIsShortString_ReturnsTrue()
    {
        var specification = TrueSpecification<String>.Identity;
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void Or_WhenCombinedWithFalse_ReturnsTrue()
    {
        var specification = TrueSpecification<String>.Identity;
        var result = specification.Or(FalseSpecification<String>.Identity).IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }
}
