using System.Linq.Expressions;
using Komair.Specifications.Abstract;
using Komair.Specifications.Abstract.Interfaces;

namespace Komair.Specifications.Internal;

internal class OrSpecification<T>(ISpecification<T> left, ISpecification<T> right) : BinarySpecificationBase<T>(left, right)
{
    protected override BinaryExpression GetBinaryExpression() => Expression.OrElse(Left.ToExpression().Body, Right.ToExpression().Body);
}
