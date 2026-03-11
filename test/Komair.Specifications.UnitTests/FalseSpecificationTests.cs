using Komair.Specifications.UnitTests.Abstract;
using NUnit.Framework;

namespace Komair.Specifications.UnitTests;

public class FalseSpecificationTests : TestBase
{
    [Test]
    public void False_AndFalse_IsFalse()
    {
        var specification = FalseSpecification<String>.Identity;
        var result = specification.And(FalseSpecification<String>.Identity).IsSatisfiedBy(ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void False_AndLongString_IsFalse()
    {
        var specification = FalseSpecification<String>.Identity;
        var result = specification.IsSatisfiedBy(LongString);

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
        var result = specification.IsSatisfiedBy(ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void False_AndTrue_IsFalse()
    {
        var specification = FalseSpecification<String>.Identity;
        var result = specification.And(TrueSpecification<String>.Identity).IsSatisfiedBy(ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void False_OrTrue_IsTrue()
    {
        var specification = FalseSpecification<String>.Identity;
        var result = specification.Or(TrueSpecification<String>.Identity).IsSatisfiedBy(ShortString);

        Assert.IsTrue(result);
    }
}
