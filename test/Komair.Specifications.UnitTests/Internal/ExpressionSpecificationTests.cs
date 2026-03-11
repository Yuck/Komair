using Komair.Specifications.UnitTests.Abstract;
using NUnit.Framework;

namespace Komair.Specifications.UnitTests.Internal;

public class ExpressionSpecificationTests : TestBase
{
    [Test]
    public void InvalidSpecification_IsFalse()
    {
        var specification = new IsShortStringSpecification();
        var result = specification.IsSatisfiedBy(LongString);

        Assert.IsFalse(result);
    }

    [Test]
    public void InvalidSpecification_WithInvalidAndLambda_IsFalse()
    {
        var specification = new IsShortStringSpecification().And(new EndsWithSpecification("xxx"));
        var result = specification.IsSatisfiedBy(LongString);

        Assert.IsFalse(result);
    }

    [Test]
    public void InvalidSpecification_WithInvalidOrLambda_IsFalse()
    {
        var specification = new IsShortStringSpecification().Or(new EndsWithSpecification("xxx"));
        var result = specification.IsSatisfiedBy(LongString);

        Assert.IsFalse(result);
    }

    [Test]
    public void InvalidSpecification_WithValidAndLambda_IsFalse()
    {
        var specification = new IsShortStringSpecification().And(new StartsWithSpecification("s"));
        var result = specification.IsSatisfiedBy(LongString);

        Assert.IsFalse(result);
    }

    [Test]
    public void InvalidSpecification_WithValidOrLambda_IsTrue()
    {
        var specification = new IsShortStringSpecification().Or(new StartsWithSpecification("a"));
        var result = specification.IsSatisfiedBy(LongString);

        Assert.IsTrue(result);
    }

    [Test]
    public void ValidSpecification_IsTrue()
    {
        var specification = new IsShortStringSpecification();
        var result = specification.IsSatisfiedBy(ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void ValidSpecification_WithInvalidAndLambda_IsFalse()
    {
        var specification = new IsShortStringSpecification().And(new EndsWithSpecification("xxx"));
        var result = specification.IsSatisfiedBy(ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void ValidSpecification_WithInvalidOrLambda_IsTrue()
    {
        var specification = new IsShortStringSpecification().Or(new EndsWithSpecification("xxx"));
        var result = specification.IsSatisfiedBy(ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void ValidSpecification_WithInvalidWhereClause_IsFalse()
    {
        var expression = new IsShortStringSpecification().Where(t => t.EndsWith("xxx"));
        var result = expression.Compile().Invoke(ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void ValidSpecification_WithValidAndLambda_IsTrue()
    {
        var specification = new IsShortStringSpecification().And(new StartsWithSpecification("s"));
        var result = specification.IsSatisfiedBy(ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void ValidSpecification_WithValidOrLambda_IsTrue()
    {
        var specification = new IsShortStringSpecification().Or(new StartsWithSpecification("s"));
        var result = specification.IsSatisfiedBy(ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void ValidSpecification_WithValidWhereClause_IsTrue()
    {
        var expression = new IsShortStringSpecification().Where(t => t.StartsWith("s"));
        var result = expression.Compile().Invoke(ShortString);

        Assert.IsTrue(result);
    }
}
