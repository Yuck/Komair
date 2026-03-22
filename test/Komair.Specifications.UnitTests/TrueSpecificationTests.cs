using Komair.Specifications.UnitTests.Abstract;
using NUnit.Framework;

namespace Komair.Specifications.UnitTests;

public class TrueSpecificationTests
{
    [Test]
    public void True_AndFalse_IsFalse()
    {
        var specification = TrueSpecification<String>.Identity;
        var result = specification.And(FalseSpecification<String>.Identity).IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void True_AndLongString_IsTrue()
    {
        var specification = TrueSpecification<String>.Identity;
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsTrue(result);
    }

    [Test]
    public void True_AndNull_IsTrue()
    {
        var specification = TrueSpecification<String>.Identity;
        var result = specification.IsSatisfiedBy(null!);

        Assert.IsTrue(result);
    }

    [Test]
    public void True_AndShortString_IsTrue()
    {
        var specification = TrueSpecification<String>.Identity;
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void True_AndTrue_IsTrue()
    {
        var specification = TrueSpecification<String>.Identity;
        var result = specification.And(TrueSpecification<String>.Identity).IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void True_OrFalse_IsTrue()
    {
        var specification = TrueSpecification<String>.Identity;
        var result = specification.Or(FalseSpecification<String>.Identity).IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void True_AndWithNoArguments_IsTrue()
    {
        var specification = TrueSpecification<String>.Identity.And();

        Assert.IsTrue(specification.IsSatisfiedBy(SpecificationBaseTests.ShortString));
    }

    [Test]
    public void True_AndTwoTrueSpecifications_IsTrue()
    {
        var specification = TrueSpecification<String>.Identity.And(TrueSpecification<String>.Identity, TrueSpecification<String>.Identity);

        Assert.IsTrue(specification.IsSatisfiedBy(SpecificationBaseTests.ShortString));
    }

    [Test]
    public void True_AndTrueAndFalse_IsFalse()
    {
        var specification = TrueSpecification<String>.Identity.And(TrueSpecification<String>.Identity, FalseSpecification<String>.Identity);

        Assert.IsFalse(specification.IsSatisfiedBy(SpecificationBaseTests.ShortString));
    }

    [Test]
    public void True_AndMultipleSpecifications_MatchesLogicalAnd()
    {
        var combined = TrueSpecification<String>.Identity.And(new SpecificationBaseTests.ContainsOrtSpecification(), new SpecificationBaseTests.IsShortStringSpecification());

        Assert.IsTrue(combined.IsSatisfiedBy(SpecificationBaseTests.ShortString));
        Assert.IsFalse(combined.IsSatisfiedBy(SpecificationBaseTests.LongString));
    }
}
