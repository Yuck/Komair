using Komair.Specifications.UnitTests.Abstract;
using NUnit.Framework;

namespace Komair.Specifications.UnitTests.Internal;

public class NotSpecificationTests
{
    [Test]
    public void Not_WhenInnerSpecificationAcceptsCandidate_ReturnsFalse()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification();
        var result = specification.Not().IsSatisfiedBy(SpecificationBaseTests.ShortString);

        Assert.IsFalse(result);
    }

    [Test]
    public void Not_WhenInnerSpecificationRejectsCandidate_ReturnsTrue()
    {
        var specification = new SpecificationBaseTests.IsShortStringSpecification();
        var result = specification.Not().IsSatisfiedBy(SpecificationBaseTests.LongString);

        Assert.IsTrue(result);
    }
}
