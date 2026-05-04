namespace Komair.Expressions.Mapping.Exceptions;

public sealed class InvalidMemberNodeException(String memberName) : Exception(CreateMessage(memberName))
{
    public String MemberName { get; } = memberName;

    private static String CreateMessage(String memberName)
    {
        ArgumentNullException.ThrowIfNull(memberName);

        return $"Unsupported member expression node: no expression is available to resolve member '{memberName}'.";
    }
}
