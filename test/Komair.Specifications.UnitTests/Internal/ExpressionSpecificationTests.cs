using Komair.Specifications.UnitTests.Abstract;
using NUnit.Framework;

namespace Komair.Specifications.UnitTests.Internal;

public class ExpressionSpecificationTests
{
    [Test]
    public void And_WhenBothPartsUnsatisfied_ReturnsFalse()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification().And(new SpecificationBaseTests.EndsWithSpecification("xxx"));
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsFalse(result);
    }

    [Test]
    public void And_WhenBothSatisfied_ReturnsTrue()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification().And(new SpecificationBaseTests.StartsWithSpecification("s"));
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void And_WhenCombinedWithUnsatisfiedRight_ReturnsFalse()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification().And(new SpecificationBaseTests.EndsWithSpecification("xxx"));
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void And_WhenLeftUnsatisfied_ReturnsFalse()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification().And(new SpecificationBaseTests.StartsWithSpecification("s"));
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsFalse(result);
    }

    [Test]
    public void IsSatisfiedBy_WhenSpecificationDoesNotMatchCandidate_ReturnsFalse()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification();
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsFalse(result);
    }

    [Test]
    public void IsSatisfiedBy_WhenSpecificationMatchesCandidate_ReturnsTrue()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification();
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void Or_WhenBothUnsatisfied_ReturnsFalse()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification().Or(new SpecificationBaseTests.EndsWithSpecification("xxx"));
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsFalse(result);
    }

    [Test]
    public void Or_WhenLeftSatisfied_ReturnsTrue()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification().Or(new SpecificationBaseTests.EndsWithSpecification("xxx"));
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void Or_WhenRightSatisfied_ReturnsTrue()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification().Or(new SpecificationBaseTests.StartsWithSpecification("a"));
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsTrue(result);
    }

    [Test]
    public void Or_WhenSecondBranchSatisfied_ReturnsTrue()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification().Or(new SpecificationBaseTests.StartsWithSpecification("s"));
        var result = specification.IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void Where_WhenAdditionalPredicateSatisfied_ReturnsTrue()
    {
        var expression = new SpecificationBaseTests.IsShortStringSpecification().Where(t => t.StartsWith('s'));
        var result = expression.Compile().Invoke(SpecificationBaseTests.ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void Where_WhenAdditionalPredicateUnsatisfied_ReturnsFalse()
    {
        var expression = new SpecificationBaseTests.IsShortStringSpecification().Where(t => t.EndsWith("xxx"));
        var result = expression.Compile().Invoke(SpecificationBaseTests.ShortString);

        Assert.IsFalse(result);
    }
}
