using System.Linq.Expressions;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode.Abstract;
using Mapster;
using LinqExpression = System.Linq.Expressions.Expression;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode;

internal class ConstantExpressionNodeMapper(TypeAdapterConfig configuration) : ExpressionNodeMapperBase<ConstantExpressionNode, ConstantExpression>(configuration)
{
    public override ConstantExpression Map(ConstantExpressionNode source)
    {
        var type = source.Type;

        if (source.Value is null)
            return LinqExpression.Constant(null, type);

        var value = source.Value.GetType() != type ? Convert.ChangeType(source.Value, type) : source.Value;
        var result = LinqExpression.Constant(value, type);

        return result;
    }
}
