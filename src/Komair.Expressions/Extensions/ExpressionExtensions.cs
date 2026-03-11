using System.Linq.Expressions;

namespace Komair.Expressions.Extensions;

public static class ExpressionExtensions
{
    public static IReadOnlyCollection<ParameterExpression> GetParameterList(this Expression expression)
    {
        return expression switch
        {
            null => [],
            BinaryExpression binary => binary.GetParameterList(),
            MemberExpression member => member.GetParameterList(),
            ParameterExpression parameter => [parameter],
            _ => []
        };
    }
}
