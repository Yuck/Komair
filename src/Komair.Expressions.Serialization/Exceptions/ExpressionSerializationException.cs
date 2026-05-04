namespace Komair.Expressions.Serialization.Exceptions;

/// <summary>
/// Thrown when expression node serialization or deserialization fails.
/// </summary>
/// <param name="message">A message that describes the error.</param>
/// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is specified.</param>
public sealed class ExpressionSerializationException(String message, Exception? innerException = null) : Exception(NonNull(message), innerException is null ? null : NonNull(innerException))
{
    private static T NonNull<T>(T value) where T : class
    {
        ArgumentNullException.ThrowIfNull(value);

        return value;
    }
}
