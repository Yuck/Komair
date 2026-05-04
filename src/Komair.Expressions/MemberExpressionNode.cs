using System.Linq.Expressions;
using Komair.Expressions.Abstract;

namespace Komair.Expressions;

/// <summary>
/// Represents a member access <see cref="System.Linq.Expressions.Expression"/> in serializable form.
/// </summary>
/// <param name="nodeType">The expression tree kind for this node.</param>
/// <param name="type">The CLR type of the expression.</param>
public class MemberExpressionNode(ExpressionType nodeType, Type type) : ExpressionNodeBase(nodeType, type)
{
    /// <summary>
    /// Gets or sets the expression that owns the member (for example the instance or parent expression).
    /// </summary>
    public required ExpressionNodeBase Expression { get; set; }

    /// <summary>
    /// Gets or sets the member name.
    /// </summary>
    public required String MemberName { get; set; }
}
