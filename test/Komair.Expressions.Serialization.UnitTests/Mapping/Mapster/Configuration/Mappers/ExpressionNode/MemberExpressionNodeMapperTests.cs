using System.Linq.Expressions;
using Komair.Expressions.Mapping.Abstract.Interfaces;
using Komair.Expressions.Mapping.Mapster;
using NUnit.Framework;

namespace Komair.Expressions.Serialization.UnitTests.Mapping.Mapster.Configuration.Mappers.ExpressionNode;

public class MemberExpressionNodeMapperTests
{
    [Test]
    public void Throws_Expected_Exceptions_For_Invalid_Member_Expression_Node()
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

        memberExpressionNode.Expression = new ConstantExpressionNode(ExpressionType.Constant, typeof(String));

        Assert.Throws<MemberAccessException>(() => mapper.ToExpression(lambdaExpressionNode));

        static IExpressionNodeMapper<Func<String, String>> GetMapper() => new MapsterExpressionNodeMapper<Func<String, String>>();
    }
}
