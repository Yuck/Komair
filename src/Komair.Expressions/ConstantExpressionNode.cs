using System.Linq.Expressions;
using Komair.Expressions.Abstract;

namespace Komair.Expressions;

/// <summary>
/// Represents a constant <see cref="System.Linq.Expressions.Expression"/> in serializable form.
/// </summary>
/// <param name="nodeType">The expression tree kind for this node.</param>
/// <param name="type">The CLR type of the expression.</param>
public class ConstantExpressionNode(ExpressionType nodeType, Type type) : ExpressionNodeBase(nodeType, type)
{
    /// <summary>
    /// Gets or sets the constant value.
    /// </summary>
    public Object? Value { get; set; }
}
