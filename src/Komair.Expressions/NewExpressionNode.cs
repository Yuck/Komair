using System.Linq.Expressions;
using Komair.Expressions.Abstract;

namespace Komair.Expressions;

/// <summary>
/// Represents a constructor call <see cref="Expression"/> in serializable form.
/// </summary>
/// <param name="nodeType">The expression tree kind for this node.</param>
/// <param name="type">The CLR type of the expression.</param>
public class NewExpressionNode(ExpressionType nodeType, Type type) : ExpressionNodeBase(nodeType, type)
{
    /// <summary>
    /// Gets or sets the constructor argument expressions.
    /// </summary>
    public required IReadOnlyCollection<ExpressionNodeBase> Arguments { get; set; }

    /// <summary>
    /// Gets or sets constructor parameter CLR types in declaration order.
    /// </summary>
    public required IReadOnlyCollection<Type> ConstructorParameterTypes { get; set; }

    /// <summary>
    /// Gets or sets optional member names associated with constructor arguments.
    /// </summary>
    public required IReadOnlyCollection<String> MemberNames { get; set; }
}
