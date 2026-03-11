using System.Linq.Expressions;
using Komair.Specifications.Abstract.Interfaces;
using Komair.Specifications.Internal;
using Komair.Specifications.Internal.ExpressionTrees;

namespace Komair.Specifications.Abstract;

public abstract class SpecificationBase<T> : ISpecification<T>
{
    private readonly Lazy<Func<T, Boolean>> _predicate;

    protected SpecificationBase()
    {
        _predicate = new Lazy<Func<T, Boolean>>(() => GetLambda(ToExpression()).Compile());
    }

    public ISpecification<T> And(ISpecification<T> specification) => new AndSpecification<T>(this, specification);

    public Boolean IsSatisfiedBy(T t) => _predicate.Value(t);

    public ISpecification<T> Not() => new NotSpecification<T>(this);

    public ISpecification<T> Or(ISpecification<T> specification) => new OrSpecification<T>(this, specification);

    public Expression<Func<T, Boolean>> Where(Expression<Func<T, Boolean>> predicate) => new AndSpecification<T>(this, new ExpressionSpecification<T>(predicate)).ToExpression();

    public abstract Expression<Func<T, Boolean>> ToExpression();

    protected static Expression<Func<T, Boolean>> GetLambda(Expression expression)
    {
        var parameters = Expression.Parameter(typeof(T));
        var body = new ParameterReplacer(parameters).Visit(expression);
        var simplified = body is Expression<Func<T, Boolean>> lambda ? lambda : Expression.Lambda<Func<T, Boolean>>(body, parameters);

        return simplified;
    }
}
