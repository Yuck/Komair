using System.Linq.Expressions;

namespace Komair.Specifications.Abstract.Interfaces;

public interface ISpecification<T>
{
    ISpecification<T> And(params ISpecification<T>[] specifications);
    Boolean IsSatisfiedBy(T t);
    ISpecification<T> Not();
    ISpecification<T> Or(params ISpecification<T>[] specifications);
    Expression<Func<T, Boolean>> ToExpression();
    Expression<Func<T, Boolean>> Where(Expression<Func<T, Boolean>> predicate);
}
