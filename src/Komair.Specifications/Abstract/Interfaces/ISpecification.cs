using System.Linq.Expressions;

namespace Komair.Specifications.Abstract.Interfaces;

/// <summary>
/// Defines a reusable predicate over <typeparamref name="T"/> that can be composed and compiled to an expression tree.
/// </summary>
/// <typeparam name="T">The type being specified.</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Combines this specification with others using logical AND.
    /// </summary>
    /// <param name="specifications">Additional specifications to combine.</param>
    /// <returns>A specification satisfied when this instance and every additional specification are satisfied.</returns>
    ISpecification<T> And(params ISpecification<T>[] specifications);

    /// <summary>
    /// Determines whether the candidate satisfies this specification.
    /// </summary>
    /// <param name="t">The candidate value.</param>
    /// <returns><see langword="true"/> when the candidate satisfies the specification; otherwise <see langword="false"/>.</returns>
    Boolean IsSatisfiedBy(T t);

    /// <summary>
    /// Returns a specification that negates this instance.
    /// </summary>
    /// <returns>A specification satisfied when this instance is not satisfied.</returns>
    ISpecification<T> Not();

    /// <summary>
    /// Combines this specification with others using logical OR.
    /// </summary>
    /// <param name="specifications">Additional specifications to combine.</param>
    /// <returns>A specification satisfied when this instance or any additional specification is satisfied.</returns>
    ISpecification<T> Or(params ISpecification<T>[] specifications);

    /// <summary>
    /// Builds the expression tree for this specification.
    /// </summary>
    /// <returns>The predicate as an expression.</returns>
    Expression<Func<T, Boolean>> ToExpression();

    /// <summary>
    /// Further restricts this specification with an additional predicate (logical AND).
    /// </summary>
    /// <param name="predicate">The additional predicate.</param>
    /// <returns>An expression combining this specification with <paramref name="predicate"/>.</returns>
    Expression<Func<T, Boolean>> Where(Expression<Func<T, Boolean>> predicate);
}
