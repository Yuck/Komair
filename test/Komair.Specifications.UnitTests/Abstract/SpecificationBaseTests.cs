using System.Linq.Expressions;
using Komair.Specifications.Abstract;
using NUnit.Framework;

namespace Komair.Specifications.UnitTests.Abstract;

public class SpecificationBaseTests
{
    public const String LongString = "a long one";
    public const String ShortString = "short";

    [Test]
    public void IsSatisfiedBy_WhenPredicateMatches_ReturnsTrue()
    {
        var specification = new ContainsLongSpecification();
        var result = specification.IsSatisfiedBy(LongString);

        Assert.IsTrue(result);
    }

    [Test]
    public void IsSatisfiedBy_WhenPredicateDoesNotMatch_ReturnsFalse()
    {
        var specification = new ContainsLongSpecification();
        var result = specification.IsSatisfiedBy(ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void And_WhenBothSatisfied_ReturnsTrue()
    {
        var left = new IsShortStringSpecification();
        var right = new ContainsOrtSpecification();
        var specification = left.And(right);
        var result = specification.IsSatisfiedBy(ShortString);

        Assert.IsTrue(result);
    }

    [Test]
    public void And_WhenLeftNotSatisfied_ReturnsFalse()
    {
        var left = new IsShortStringSpecification();
        var right = new ContainsLongSpecification();
        var specification = left.And(right);
        var result = specification.IsSatisfiedBy(LongString);

        Assert.IsFalse(result);
    }

    [Test]
    public void Or_WhenEitherSatisfied_ReturnsTrue()
    {
        var left = new IsShortStringSpecification();
        var right = new ContainsLongSpecification();
        var specification = left.Or(right);
        var result = specification.IsSatisfiedBy(LongString);

        Assert.IsTrue(result);
    }

    [Test]
    public void Not_WhenSatisfied_ReturnsFalse()
    {
        var specification = new IsShortStringSpecification();
        var result = specification.Not().IsSatisfiedBy(ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void Where_WhenPredicateMatches_ReturnsTrue()
    {
        var specification = new IsShortStringSpecification();
        var expression = specification.Where(t => t.StartsWith("s"));
        var result = expression.Compile().Invoke(ShortString);

        Assert.IsTrue(result);
    }

    public class ContainsLongSpecification : SpecificationBase<String>
    {
        public override Expression<Func<String, Boolean>> ToExpression()
        {
            return t => t.Contains("long");
        }
    }

    public class ContainsOrtSpecification : SpecificationBase<String>
    {
        public override Expression<Func<String, Boolean>> ToExpression()
        {
            return t => t.Contains("ort");
        }
    }

    public class EndsWithSpecification(String suffix) : SpecificationBase<String>
    {
        private readonly String _suffix = suffix;

        public override Expression<Func<String, Boolean>> ToExpression()
        {
            return t => t.EndsWith(_suffix);
        }
    }

    public class IsShortStringSpecification : SpecificationBase<String>
    {
        public override Expression<Func<String, Boolean>> ToExpression()
        {
            return t => t.Length < 10;
        }
    }

    public class StartsWithSpecification(String prefix) : SpecificationBase<String>
    {
        public override Expression<Func<String, Boolean>> ToExpression()
        {
            return t => t.StartsWith(prefix);
        }
    }
}
