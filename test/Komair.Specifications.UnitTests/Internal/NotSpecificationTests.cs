using Komair.Specifications.UnitTests.Abstract;
using NUnit.Framework;

namespace Komair.Specifications.UnitTests.Internal;

public class NotSpecificationTests : TestBase
{
    [Test]
    public void InvalidSpecification_WhenNegated_IsTrue()
    {
        var specification = new IsShortStringSpecification();
        var result = specification.Not().IsSatisfiedBy(LongString);

        Assert.IsTrue(result);
    }

    [Test]
    public void ValidSpecification_WhenNegated_IsFalse()
    {
        var specification = new IsShortStringSpecification();
        var result = specification.Not().IsSatisfiedBy(ShortString);

        Assert.IsFalse(result);
    }
}
