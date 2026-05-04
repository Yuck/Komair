using System.Linq.Expressions;
using Komair.Expressions.Abstract;

namespace Komair.Expressions;

/// <summary>
/// Represents a binary <see cref="System.Linq.Expressions.Expression"/> in serializable form.
/// </summary>
/// <param name="nodeType">The expression tree kind for this node.</param>
/// <param name="type">The CLR type of the expression.</param>
public class BinaryExpressionNode(ExpressionType nodeType, Type type) : ExpressionNodeBase(nodeType, type)
{
    /// <summary>
    /// Gets or sets the left operand.
    /// </summary>
    public required ExpressionNodeBase Left { get; set; }

    /// <summary>
    /// Gets or sets the right operand.
    /// </summary>
    public required ExpressionNodeBase Right { get; set; }
}
