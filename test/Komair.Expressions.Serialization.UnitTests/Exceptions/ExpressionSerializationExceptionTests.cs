using Komair.Expressions.Serialization.Exceptions;
using NUnit.Framework;

namespace Komair.Expressions.Serialization.UnitTests.Exceptions;

public class ExpressionSerializationExceptionTests
{
    [Test]
    public void Constructor_WhenInnerExceptionProvided_SetsInnerException()
    {
        var inner = new InvalidOperationException("inner");

        var exception = new ExpressionSerializationException("serialization failed", inner);

        Assert.AreSame(inner, exception.InnerException);
        Assert.That(exception.Message, Does.Contain("serialization failed"));
    }

    [Test]
    public void Constructor_WhenMessageNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ExpressionSerializationException(null!));
    }

    [Test]
    public void Constructor_WhenOnlyMessageProvided_HasNoInnerException()
    {
        var exception = new ExpressionSerializationException("only message");

        Assert.That(exception.Message, Does.Contain("only message"));
        Assert.IsNull(exception.InnerException);
    }
}
