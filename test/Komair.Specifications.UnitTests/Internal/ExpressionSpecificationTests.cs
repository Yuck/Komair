using Komair.Specifications.UnitTests.Abstract;
using NUnit.Framework;

namespace Komair.Specifications.UnitTests.Internal;

public class ExpressionSpecificationTests
{
    [Test]
    public void InvalidSpecification_IsFalse()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification();
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsFalse(result);
    }

    [Test]
    public void InvalidSpecification_WithInvalidAndLambda_IsFalse()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification().And(new SpecificationBaseTests.EndsWithSpecification("xxx"));
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsFalse(result);
    }

    [Test]
    public void InvalidSpecification_WithInvalidOrLambda_IsFalse()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification().Or(new SpecificationBaseTests.EndsWithSpecification("xxx"));
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsFalse(result);
    }

    [Test]
    public void InvalidSpecification_WithValidAndLambda_IsFalse()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification().And(new SpecificationBaseTests.StartsWithSpecification("s"));
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsFalse(result);
    }

    [Test]
    public void InvalidSpecification_WithValidOrLambda_IsTrue()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification().Or(new SpecificationBaseTests.StartsWithSpecification("a"));
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsTrue(result);
    }

    [Test]
    public void ValidSpecification_IsTrue()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification();
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void ValidSpecification_WithInvalidAndLambda_IsFalse()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification().And(new SpecificationBaseTests.EndsWithSpecification("xxx"));
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void ValidSpecification_WithInvalidOrLambda_IsTrue()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification().Or(new SpecificationBaseTests.EndsWithSpecification("xxx"));
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void ValidSpecification_WithInvalidWhereClause_IsFalse()
    {
        var expression = new SpecificationBaseTests.IsShortStringSpecification().Where(t => t.EndsWith("xxx"));
        var result = expression.Compile().Invoke(SpecificationBaseTests.ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void ValidSpecification_WithValidAndLambda_IsTrue()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification().And(new SpecificationBaseTests.StartsWithSpecification("s"));
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void ValidSpecification_WithValidOrLambda_IsTrue()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification().Or(new SpecificationBaseTests.StartsWithSpecification("s"));
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void ValidSpecification_WithValidWhereClause_IsTrue()
    {
        var expression = new SpecificationBaseTests.IsShortStringSpecification().Where(t => t.StartsWith("s"));
        var result = expression.Compile().Invoke(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }
}
