using System.Linq.Expressions;
using Komair.Specifications.Abstract.Interfaces;

namespace Komair.Specifications.Abstract;

/// <summary>
/// Base class for specifications that combine exactly two child specifications.
/// </summary>
/// <typeparam name="T">The type being specified.</typeparam>
/// <param name="left">The left operand specification.</param>
/// <param name="right">The right operand specification.</param>
public abstract class BinarySpecificationBase<T>(ISpecification<T> left, ISpecification<T> right) : SpecificationBase<T>
{
    /// <summary>
    /// The left operand specification.
    /// </summary>
    protected ISpecification<T> Left = left;

    /// <summary>
    /// The right operand specification.
    /// </summary>
    protected ISpecification<T> Right = right;

    /// <inheritdoc />
    public override Expression<Func<T, Boolean>> ToExpression()
    {
        return GetLambda(GetBinaryExpression());
    }

    /// <summary>
    /// When implemented, returns the binary expression that combines <see cref="Left"/> and <see cref="Right"/>.
    /// </summary>
    /// <returns>The combined binary expression.</returns>
    protected abstract BinaryExpression GetBinaryExpression();
}
