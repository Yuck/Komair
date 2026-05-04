namespace Komair.Expressions.Serialization.Exceptions;

public sealed class ExpressionSerializationException(String message, Exception? innerException = null) : Exception(NonNull(message), innerException is null ? null : NonNull(innerException))
{
    private static T NonNull<T>(T value) where T : class
    {
        ArgumentNullException.ThrowIfNull(value);

        return value;
    }
}
