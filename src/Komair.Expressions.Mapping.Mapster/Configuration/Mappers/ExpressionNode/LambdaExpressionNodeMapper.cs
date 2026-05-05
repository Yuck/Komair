using System.Linq.Expressions;
using Komair.Expressions.Extensions;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode.Abstract;
using Mapster;
using LinqExpression = System.Linq.Expressions.Expression;

namespace Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode;

internal class LambdaExpressionNodeMapper<T>(TypeAdapterConfig configuration) : ExpressionNodeMapperBase<LambdaExpressionNode, Expression<T>>(configuration)
{
    public override Expression<T> Map(LambdaExpressionNode source)
    {
        var body = source.Body.Adapt<LinqExpression>(Configuration);
        var bodyParameters = body.GetParameterList();
        var parameters = source.Parameters.Select(MapParameter).ToArray();
        var result = LinqExpression.Lambda<T>(body, parameters);

        return result;

        ParameterExpression MapParameter(ParameterExpressionNode sourceParameter)
        {
            var parameter = bodyParameters.FirstOrDefault(t => t.Type == sourceParameter.Type && t.Name == sourceParameter.Name);
            var expression = parameter ?? sourceParameter.Adapt<ParameterExpression>(Configuration);

            return expression;
        }
    }
}
