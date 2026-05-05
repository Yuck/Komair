using Komair.Expressions.Abstract;

namespace Komair.Expressions;

/// <summary>
/// Represents a collection element initializer.
/// </summary>
public class ElementInitNode
{
    /// <summary>
    /// Gets or sets the method name used for the element initialization.
    /// </summary>
    public required String AddMethodName { get; set; }

    /// <summary>
    /// Gets or sets the element initializer arguments.
    /// </summary>
    public required IReadOnlyCollection<ExpressionNodeBase> Arguments { get; set; }
}
