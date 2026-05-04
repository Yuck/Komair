namespace Komair.Expressions.Mapping.Exceptions;

/// <summary>
/// Thrown when a member expression node cannot be mapped because no qualifying inner expression is present.
/// </summary>
/// <param name="memberName">The member name that could not be resolved.</param>
public sealed class InvalidMemberNodeException(String memberName) : Exception(CreateMessage(memberName))
{
    /// <summary>
    /// Gets the member name from the invalid node.
    /// </summary>
    public String MemberName { get; } = memberName;

    private static String CreateMessage(String memberName)
    {
        ArgumentNullException.ThrowIfNull(memberName);

        return $"Unsupported member expression node: no expression is available to resolve member '{memberName}'.";
    }
}
