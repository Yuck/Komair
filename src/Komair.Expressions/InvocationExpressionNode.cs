using System.Linq.Expressions;
using Komair.Expressions.Abstract;

namespace Komair.Expressions;

/// <summary>
/// Represents an invocation <see cref="System.Linq.Expressions.Expression"/> in serializable form.
/// </summary>
/// <param name="nodeType">The expression tree kind for this node.</param>
/// <param name="type">The CLR type of the expression.</param>
public class InvocationExpressionNode(ExpressionType nodeType, Type type) : ExpressionNodeBase(nodeType, type)
{
    /// <summary>
    /// Gets or sets the target expression to invoke.
    /// </summary>
    public required ExpressionNodeBase Expression { get; set; }

    /// <summary>
    /// Gets or sets invocation argument expressions.
    /// </summary>
    public required IReadOnlyCollection<ExpressionNodeBase> Arguments { get; set; }
}
