using Komair.Specifications.UnitTests.Abstract;
using NUnit.Framework;

namespace Komair.Specifications.UnitTests;

public class FalseSpecificationTests
{
    [Test]
    public void And_WhenCombinedWithFalse_ReturnsFalse()
    {
        var specification = FalseSpecification<String>.Identity;
        var result = specification.And(FalseSpecification<String>.Identity).IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void And_WhenCombinedWithTrue_ReturnsFalse()
    {
        var specification = FalseSpecification<String>.Identity;
        var result = specification.And(TrueSpecification<String>.Identity).IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void IsSatisfiedBy_WhenCandidateIsLongString_ReturnsFalse()
    {
        var specification = FalseSpecification<String>.Identity;
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsFalse(result);
    }

    [Test]
    public void IsSatisfiedBy_WhenCandidateIsNull_ReturnsFalse()
    {
        var specification = FalseSpecification<String>.Identity;
        var result = specification.IsSatisfiedBy(null!);

        Assert.IsFalse(result);
    }

    [Test]
    public void IsSatisfiedBy_WhenCandidateIsShortString_ReturnsFalse()
    {
        var specification = FalseSpecification<String>.Identity;
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void Or_WhenCandidateMatchesAtLeastOne_ReturnsTrue()
    {
        var combined = FalseSpecification<String>.Identity.Or(new SpecificationBaseTests.IsShortStringSpecification(), new SpecificationBaseTests.ContainsLongSpecification());

        Assert.IsTrue(combined.IsSatisfiedBy(SpecificationBaseTests.ShortString));
        Assert.IsTrue(combined.IsSatisfiedBy(SpecificationBaseTests.LongString));
    }

    [Test]
    public void Or_WhenCandidateMatchesNeither_ReturnsFalse()
    {
        var combined = FalseSpecification<String>.Identity.Or(new SpecificationBaseTests.IsShortStringSpecification(), new SpecificationBaseTests.ContainsLongSpecification());

        Assert.IsFalse(combined.IsSatisfiedBy("1234567890"));
    }

    [Test]
    public void Or_WhenCombinedWithTrue_ReturnsTrue()
    {
        var specification = FalseSpecification<String>.Identity;
        var result = specification.Or(TrueSpecification<String>.Identity).IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void Or_WhenGivenTwoFalseSpecifications_ReturnsFalse()
    {
        var specification = FalseSpecification<String>.Identity.Or(FalseSpecification<String>.Identity, FalseSpecification<String>.Identity);

        Assert.IsFalse(specification.IsSatisfiedBy(SpecificationBaseTests.ShortString));
    }

    [Test]
    public void Or_WhenIncludesTrue_ReturnsTrue()
    {
        var specification = FalseSpecification<String>.Identity.Or(FalseSpecification<String>.Identity, TrueSpecification<String>.Identity);

        Assert.IsTrue(specification.IsSatisfiedBy(SpecificationBaseTests.ShortString));
    }

    [Test]
    public void Or_WhenNoAdditionalSpecifications_ReturnsFalse()
    {
        var specification = FalseSpecification<String>.Identity.Or();

        Assert.IsFalse(specification.IsSatisfiedBy(SpecificationBaseTests.ShortString));
    }
}
