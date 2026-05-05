using Komair.Expressions.Abstract;

namespace Komair.Expressions;

/// <summary>
/// Represents a member assignment used in a member-initializer expression.
/// </summary>
public class MemberAssignmentNode
{
    /// <summary>
    /// Gets or sets the member name.
    /// </summary>
    public required String MemberName { get; set; }

    /// <summary>
    /// Gets or sets the assignment expression.
    /// </summary>
    public required ExpressionNodeBase Expression { get; set; }
}
