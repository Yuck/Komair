using Komair.Specifications.UnitTests.Abstract;
using NUnit.Framework;

namespace Komair.Specifications.UnitTests;

public class FalseSpecificationTests
{
    [Test]
    public void False_AndFalse_IsFalse()
    {
        var specification = FalseSpecification<String>.Identity;
        var result = specification.And(FalseSpecification<String>.Identity).IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void False_AndLongString_IsFalse()
    {
        var specification = FalseSpecification<String>.Identity;
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsFalse(result);
    }

    [Test]
    public void False_AndNull_IsFalse()
    {
        var specification = FalseSpecification<String>.Identity;
        var result = specification.IsSatisfiedBy(null!);

        Assert.IsFalse(result);
    }

    [Test]
    public void False_AndShortString_IsFalse()
    {
        var specification = FalseSpecification<String>.Identity;
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void False_AndTrue_IsFalse()
    {
        var specification = FalseSpecification<String>.Identity;
        var result = specification.And(TrueSpecification<String>.Identity).IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void False_OrTrue_IsTrue()
    {
        var specification = FalseSpecification<String>.Identity;
        var result = specification.Or(TrueSpecification<String>.Identity).IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void False_OrWithNoArguments_IsFalse()
    {
        var specification = FalseSpecification<String>.Identity.Or();

        Assert.IsFalse(specification.IsSatisfiedBy(SpecificationBaseTests.ShortString));
    }

    [Test]
    public void False_OrTwoFalseSpecifications_IsFalse()
    {
        var specification = FalseSpecification<String>.Identity.Or(FalseSpecification<String>.Identity, FalseSpecification<String>.Identity);

        Assert.IsFalse(specification.IsSatisfiedBy(SpecificationBaseTests.ShortString));
    }

    [Test]
    public void False_OrFalseAndTrue_IsTrue()
    {
        var specification = FalseSpecification<String>.Identity.Or(FalseSpecification<String>.Identity, TrueSpecification<String>.Identity);

        Assert.IsTrue(specification.IsSatisfiedBy(SpecificationBaseTests.ShortString));
    }

    [Test]
    public void False_OrMultipleSpecifications_MatchesLogicalOr()
    {
        var combined = FalseSpecification<String>.Identity.Or(new SpecificationBaseTests.IsShortStringSpecification(), new SpecificationBaseTests.ContainsLongSpecification());

        Assert.IsTrue(combined.IsSatisfiedBy(SpecificationBaseTests.ShortString));
        Assert.IsTrue(combined.IsSatisfiedBy(SpecificationBaseTests.LongString));
        Assert.IsFalse(combined.IsSatisfiedBy("1234567890"));
    }
}
