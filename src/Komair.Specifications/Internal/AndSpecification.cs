using System.Linq.Expressions;
using Komair.Specifications.Abstract;
using Komair.Specifications.Abstract.Interfaces;

namespace Komair.Specifications.Internal;

internal class AndSpecification<T>(ISpecification<T> left, ISpecification<T> right) : BinarySpecificationBase<T>(left, right)
{
    protected override BinaryExpression GetBinaryExpression() => Expression.AndAlso(Left.ToExpression().Body, Right.ToExpression().Body);
}
