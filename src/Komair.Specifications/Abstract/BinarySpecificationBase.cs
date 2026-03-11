using System.Linq.Expressions;
using Komair.Specifications.Abstract.Interfaces;

namespace Komair.Specifications.Abstract;

public abstract class BinarySpecificationBase<T>(ISpecification<T> left, ISpecification<T> right) : SpecificationBase<T>
{
    protected ISpecification<T> Left = left ?? throw new ArgumentNullException(nameof(left));
    protected ISpecification<T> Right = right ?? throw new ArgumentNullException(nameof(right));

    public override Expression<Func<T, Boolean>> ToExpression() => GetLambda(GetBinaryExpression());

    protected abstract BinaryExpression GetBinaryExpression();
}
