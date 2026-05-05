using System.Linq.Expressions;
using Komair.Expressions.Abstract;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.Expression;
using Komair.Expressions.Mapping.Mapster.Configuration.Mappers.ExpressionNode;
using Mapster;

namespace Komair.Expressions.Mapping.Mapster.Configuration;

internal class DefaultTypeAdapterConfiguration<T> : TypeAdapterConfig
{
    public DefaultTypeAdapterConfiguration()
    {
        MapLinqExpressionToExpressionNodes();
        MapExpressionNodesToLinqExpression();
    }

    private void MapExpressionNodesToLinqExpression()
    {
        ForType<BinaryExpressionNode, Expression>().MapWith(source => new BinaryExpressionNodeMapper(this).Map(source));
        ForType<BlockExpressionNode, Expression>().MapWith(source => new BlockExpressionNodeMapper(this).Map(source));
        ForType<ConditionalExpressionNode, Expression>().MapWith(source => new ConditionalExpressionNodeMapper(this).Map(source));
        ForType<ConstantExpressionNode, Expression>().MapWith(source => new ConstantExpressionNodeMapper(this).Map(source));
        ForType<ExpressionNodeBase, Expression<T>>().MapWith(source => new ExpressionNodeMapper<T>(this).Map(source));
        ForType<InvocationExpressionNode, Expression>().MapWith(source => new InvocationExpressionNodeMapper(this).Map(source));
        ForType<ListInitExpressionNode, Expression>().MapWith(source => new ListInitExpressionNodeMapper(this).Map(source));
        ForType<MemberExpressionNode, Expression>().MapWith(source => new MemberExpressionNodeMapper(this).Map(source));
        ForType<MemberInitExpressionNode, Expression>().MapWith(source => new MemberInitExpressionNodeMapper(this).Map(source));
        ForType<NewExpressionNode, Expression>().MapWith(source => new NewExpressionNodeMapper(this).Map(source));
        ForType<NewExpressionNode, NewExpression>().MapWith(source => new NewExpressionNodeMapper(this).Map(source));
        ForType<ParameterExpressionNode, Expression>().MapWith(source => new ParameterExpressionNodeMapper(this).Map(source));
        ForType<ParameterExpressionNode, ParameterExpression>().MapWith(source => new ParameterExpressionNodeMapper(this).Map(source));
        ForType<QuoteExpressionNode, Expression>().MapWith(source => new QuoteExpressionNodeMapper(this).Map(source));
    }

    private void MapLinqExpressionToExpressionNodes()
    {
        ForType<BinaryExpression, ExpressionNodeBase>().MapWith(source => new BinaryExpressionMapper(this).Map(source));
        ForType<BlockExpression, BlockExpressionNode>().MapWith(source => new BlockExpressionMapper(this).Map(source));
        ForType<BlockExpression, ExpressionNodeBase>().MapWith(source => new BlockExpressionMapper(this).Map(source));
        ForType<ConditionalExpression, ConditionalExpressionNode>().MapWith(source => new ConditionalExpressionMapper(this).Map(source));
        ForType<ConditionalExpression, ExpressionNodeBase>().MapWith(source => new ConditionalExpressionMapper(this).Map(source));
        ForType<ConstantExpression, ExpressionNodeBase>().MapWith(source => new ConstantExpressionMapper(this).Map(source));
        ForType<Expression<T>, ExpressionNodeBase>().MapWith(source => new ExpressionMapper<T>(this).Map(source));
        ForType<InvocationExpression, ExpressionNodeBase>().MapWith(source => new InvocationExpressionMapper(this).Map(source));
        ForType<InvocationExpression, InvocationExpressionNode>().MapWith(source => new InvocationExpressionMapper(this).Map(source));
        ForType<LambdaExpression, ExpressionNodeBase>().MapWith(source => new LambdaExpressionMapper(this).Map(source));
        ForType<LambdaExpression, LambdaExpressionNode>().MapWith(source => new LambdaExpressionMapper(this).Map(source));
        ForType<ListInitExpression, ExpressionNodeBase>().MapWith(source => new ListInitExpressionMapper(this).Map(source));
        ForType<ListInitExpression, ListInitExpressionNode>().MapWith(source => new ListInitExpressionMapper(this).Map(source));
        ForType<MemberExpression, ExpressionNodeBase>().MapWith(source => new MemberExpressionMapper(this).Map(source));
        ForType<MemberInitExpression, ExpressionNodeBase>().MapWith(source => new MemberInitExpressionMapper(this).Map(source));
        ForType<MemberInitExpression, MemberInitExpressionNode>().MapWith(source => new MemberInitExpressionMapper(this).Map(source));
        ForType<NewExpression, ExpressionNodeBase>().MapWith(source => new NewExpressionMapper(this).Map(source));
        ForType<NewExpression, NewExpressionNode>().MapWith(source => new NewExpressionMapper(this).Map(source));
        ForType<ParameterExpression, ExpressionNodeBase>().MapWith(source => new ParameterExpressionMapper(this).Map(source));
        ForType<ParameterExpression, ParameterExpressionNode>().MapWith(source => new ParameterExpressionMapper(this).Map(source));
        ForType<UnaryExpression, ExpressionNodeBase>().MapWith(source => new QuoteExpressionMapper(this).Map(source));
        ForType<UnaryExpression, QuoteExpressionNode>().MapWith(source => new QuoteExpressionMapper(this).Map(source));
    }
}
