using System.Linq.Expressions;
using Komair.Expressions.Abstract;

namespace Komair.Expressions;

/// <summary>
/// Represents a conditional <see cref="System.Linq.Expressions.Expression"/> in serializable form.
/// </summary>
/// <param name="nodeType">The expression tree kind for this node.</param>
/// <param name="type">The CLR type of the expression.</param>
public class ConditionalExpressionNode(ExpressionType nodeType, Type type) : ExpressionNodeBase(nodeType, type)
{
    /// <summary>
    /// Gets or sets the test expression.
    /// </summary>
    public required ExpressionNodeBase Test { get; set; }

    /// <summary>
    /// Gets or sets the expression evaluated when the test is true.
    /// </summary>
    public required ExpressionNodeBase IfTrue { get; set; }

    /// <summary>
    /// Gets or sets the expression evaluated when the test is false.
    /// </summary>
    public required ExpressionNodeBase IfFalse { get; set; }
}
