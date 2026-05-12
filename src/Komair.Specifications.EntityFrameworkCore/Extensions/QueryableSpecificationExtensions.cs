using System.Linq.Expressions;
using Komair.Specifications.Abstract.Interfaces;

namespace Komair.Specifications.EntityFrameworkCore.Extensions;

/// <summary>
/// Adds convenience helpers for applying Komair specifications to queryable data sources.
/// </summary>
/// <remarks>
/// <para>
/// Use <see cref="Where{T}(IQueryable{T}, ISpecification{T})"/> when a filter always applies. Use <see cref="WhereIf{T}(IQueryable{T}, Boolean, ISpecification{T})"/> or <see cref="WhereIf{T}(IQueryable{T}, Boolean, Expression{Func{T, Boolean}})"/> when you want one fluent chain and each filter may be skipped based on runtime state (optional search fields, feature toggles, and similar).
/// </para>
/// </remarks>
public static class QueryableSpecificationExtensions
{
    /// <summary>
    /// Applies a specification expression to the query source.
    /// </summary>
    /// <typeparam name="T">The element type of the query source.</typeparam>
    /// <param name="query">The query source.</param>
    /// <param name="specification">The specification to apply.</param>
    /// <returns>A query filtered by the specification expression.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="query"/> or <paramref name="specification"/> is <see langword="null"/>.</exception>
    public static IQueryable<T> Where<T>(this IQueryable<T> query, ISpecification<T> specification)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(specification);

        return Queryable.Where(query, specification.ToExpression());
    }

    /// <summary>
    /// Applies a specification expression to the query source only when <paramref name="condition"/> is <see langword="true"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Prefer this method over branching with separate <c>Where</c> calls when you are composing one query from many optional filters: each segment stays in method-chain order without intermediate locals.
    /// </para>
    /// <para>
    /// When the filter is unconditional, call <see cref="Where{T}(IQueryable{T}, ISpecification{T})"/> instead of passing a constant <see langword="true"/> <paramref name="condition"/>.
    /// </para>
    /// <para>
    /// Arguments are evaluated before this method runs: <paramref name="specification"/> must not be <see langword="null"/> even when <paramref name="condition"/> is <see langword="false"/>, and any cost of constructing the specification is still paid unless the caller avoids invoking this overload (for example by branching before the chain).
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The element type of the query source.</typeparam>
    /// <param name="query">The query source.</param>
    /// <param name="condition">When <see langword="true"/>, the specification is applied; when <see langword="false"/>, <paramref name="query"/> is returned unchanged.</param>
    /// <param name="specification">The specification to apply when <paramref name="condition"/> is <see langword="true"/>.</param>
    /// <returns>The original query when <paramref name="condition"/> is <see langword="false"/>; otherwise the filtered query.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="query"/> or <paramref name="specification"/> is <see langword="null"/>.</exception>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, Boolean condition, ISpecification<T> specification)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(specification);

        return condition ? Queryable.Where(query, specification.ToExpression()) : query;
    }

    /// <summary>
    /// Applies a predicate expression to the query source only when <paramref name="condition"/> is <see langword="true"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Prefer this method over branching with separate <c>Where</c> calls when you are composing one query from many optional filters: each segment stays in method-chain order without intermediate locals.
    /// </para>
    /// <para>
    /// When the filter is unconditional, call <see cref="Queryable.Where{TSource}(IQueryable{TSource}, Expression{Func{TSource, Boolean}})"/> instead of passing a constant <see langword="true"/> <paramref name="condition"/>.
    /// </para>
    /// <para>
    /// Arguments are evaluated before this method runs: <paramref name="predicate"/> must not be <see langword="null"/> even when <paramref name="condition"/> is <see langword="false"/>.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The element type of the query source.</typeparam>
    /// <param name="query">The query source.</param>
    /// <param name="condition">When <see langword="true"/>, the predicate is applied; when <see langword="false"/>, <paramref name="query"/> is returned unchanged.</param>
    /// <param name="predicate">The predicate to apply when <paramref name="condition"/> is <see langword="true"/>.</param>
    /// <returns>The original query when <paramref name="condition"/> is <see langword="false"/>; otherwise the filtered query.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="query"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, Boolean condition, Expression<Func<T, Boolean>> predicate)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(predicate);

        return condition ? Queryable.Where(query, predicate) : query;
    }
}
