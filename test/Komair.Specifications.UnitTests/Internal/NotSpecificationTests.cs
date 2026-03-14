using Komair.Specifications.UnitTests.Abstract;
using NUnit.Framework;

namespace Komair.Specifications.UnitTests.Internal;

public class NotSpecificationTests
{
    [Test]
    public void InvalidSpecification_WhenNegated_IsTrue()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification();
        var result = specification.Not().IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsTrue(result);
    }

    [Test]
    public void ValidSpecification_WhenNegated_IsFalse()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification();
        var result = specification.Not().IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsFalse(result);
    }
}
