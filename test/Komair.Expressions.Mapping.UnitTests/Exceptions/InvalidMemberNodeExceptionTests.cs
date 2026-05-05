using Komair.Expressions.Mapping.Exceptions;
using NUnit.Framework;

namespace Komair.Expressions.Mapping.UnitTests.Exceptions;

public class InvalidMemberNodeExceptionTests
{
    [Test]
    public void Constructor_WhenMemberNameNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new InvalidMemberNodeException(null!));
    }

    [Test]
    public void Constructor_WhenMemberNameProvided_SetsMemberNameAndMessage()
    {
        var exception = new InvalidMemberNodeException("SomeMember");

        Assert.AreEqual("SomeMember", exception.MemberName);
        Assert.That(exception.Message, Does.Contain("SomeMember"));
    }
}
