using System.Linq.Expressions;

namespace Komair.Specifications.Abstract.Interfaces;

public interface ISpecification<T>
{
    Boolean IsSatisfiedBy(T t);

    ISpecification<T> And(ISpecification<T> specification);
    ISpecification<T> Not();
    ISpecification<T> Or(ISpecification<T> specification);
    Expression<Func<T, Boolean>> ToExpression();
}
