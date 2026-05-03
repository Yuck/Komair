using System.Linq.Expressions;
using Komair.Expressions.Mapping.Abstract.Interfaces;
using NUnit.Framework;

namespace Komair.Expressions.Mapping.Mapster.UnitTests.Configuration.Mappers.ExpressionNode;

public class MemberExpressionNodeMapperTests
{
    [Test]
    public void ToExpression_WhenMemberAccessExpressionIsConstant_ThrowsMemberAccessException()
    {
        var expressionType = ExpressionType.MemberAccess;
        var memberExpressionNode = new MemberExpressionNode(expressionType, typeof(String))
        {
            Expression = new ConstantExpressionNode(ExpressionType.Constant, typeof(String)),
            MemberName = "HelloWorld"
        };
        var lambdaExpressionNode = new LambdaExpressionNode(ExpressionType.Lambda, typeof(Func<String, String>))
        {
            Body = memberExpressionNode,
            Parameters = []
        };

        var mapper = GetMapper();

        Assert.Throws<MemberAccessException>(() => mapper.ToExpression(lambdaExpressionNode));

        static IExpressionNodeMapper<Func<String, String>> GetMapper() => new MapsterExpressionNodeMapper<Func<String, String>>();
    }

    [Test]
    public void ToExpression_WhenMemberAccessExpressionNull_ThrowsNullReferenceException()
    {
        var expressionType = ExpressionType.MemberAccess;
        var memberExpressionNode = new MemberExpressionNode(expressionType, typeof(String))
        {
            Expression = null!,
            MemberName = "HelloWorld"
        };
        var lambdaExpressionNode = new LambdaExpressionNode(ExpressionType.Lambda, typeof(Func<String, String>))
        {
            Body = memberExpressionNode,
            Parameters = []
        };

        var mapper = GetMapper();

        Assert.Throws<NullReferenceException>(() => mapper.ToExpression(lambdaExpressionNode));

        static IExpressionNodeMapper<Func<String, String>> GetMapper() => new MapsterExpressionNodeMapper<Func<String, String>>();
    }
}
