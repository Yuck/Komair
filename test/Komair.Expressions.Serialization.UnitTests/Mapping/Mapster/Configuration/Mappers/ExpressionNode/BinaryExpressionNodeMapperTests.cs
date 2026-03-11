using System.Linq.Expressions;
using Komair.Expressions.Mapping.Abstract.Interfaces;
using Komair.Expressions.Mapping.Mapster;
using NUnit.Framework;

namespace Komair.Expressions.Serialization.UnitTests.Mapping.Mapster.Configuration.Mappers.ExpressionNode;

public class BinaryExpressionNodeMapperTests
{
    [Test]
    public void Maps_Int_To_Int_Operations()
    {
        var operations = new[]
        {
            ExpressionType.Add, ExpressionType.AddAssign, ExpressionType.AddAssignChecked,
            ExpressionType.AddChecked, ExpressionType.And, ExpressionType.AndAssign,
            ExpressionType.Assign, ExpressionType.Divide, ExpressionType.DivideAssign,
            ExpressionType.ExclusiveOr, ExpressionType.ExclusiveOrAssign, ExpressionType.LeftShift, ExpressionType.LeftShiftAssign,
            ExpressionType.Modulo, ExpressionType.ModuloAssign, ExpressionType.Multiply, ExpressionType.MultiplyAssign, ExpressionType.MultiplyAssignChecked,
            ExpressionType.MultiplyChecked, ExpressionType.Or, ExpressionType.OrAssign,
            ExpressionType.RightShift, ExpressionType.RightShiftAssign, ExpressionType.Subtract, ExpressionType.SubtractAssign,
            ExpressionType.SubtractAssignChecked, ExpressionType.SubtractChecked
        };
        var mapper = GetMapper();
        var parameter = new ParameterExpressionNode(ExpressionType.Parameter, typeof(Int32)) { Name = "t" };

        foreach (var operation in operations)
        {
            var node = new LambdaExpressionNode(ExpressionType.Lambda, typeof(Func<Int32, Int32>))
            {
                Body = new BinaryExpressionNode(operation, typeof(Int32)) { Left = parameter, Right = parameter },
                Parameters = new List<ParameterExpressionNode> { parameter }
            };
            var expression = mapper.ToExpression(node);

            Assert.IsNotNull(expression);
        }

        static IExpressionNodeMapper<Func<Int32, Int32>> GetMapper() => new MapsterExpressionNodeMapper<Func<Int32, Int32>>();
    }

    [Test]
    public void Maps_Int_To_Bool_Operations()
    {
        var operations = new[]
        {
            ExpressionType.Equal, ExpressionType.GreaterThan, ExpressionType.GreaterThanOrEqual,
            ExpressionType.LessThan, ExpressionType.LessThanOrEqual, ExpressionType.NotEqual
        };
        var mapper = GetMapper();
        var parameter = new ParameterExpressionNode(ExpressionType.Parameter, typeof(Int32)) { Name = "t" };

        foreach (var operation in operations)
        {
            var node = new LambdaExpressionNode(ExpressionType.Lambda, typeof(Func<Int32, Boolean>))
            {
                Body = new BinaryExpressionNode(operation, typeof(Int32)) { Left = parameter, Right = parameter },
                Parameters = new List<ParameterExpressionNode> { parameter }
            };
            var expression = mapper.ToExpression(node);

            Assert.IsNotNull(expression);
        }

        static IExpressionNodeMapper<Func<Int32, Boolean>> GetMapper() => new MapsterExpressionNodeMapper<Func<Int32, Boolean>>();
    }

    [Test]
    public void Maps_Bool_To_Bool_Operations()
    {
        var operations = new[]
        {
            ExpressionType.AndAlso, ExpressionType.OrElse
        };
        var mapper = GetMapper();
        var parameter = new ParameterExpressionNode(ExpressionType.Parameter, typeof(Boolean)) { Name = "t" };

        foreach (var operation in operations)
        {
            var node = new LambdaExpressionNode(ExpressionType.Lambda, typeof(Func<Boolean, Boolean>))
            {
                Body = new BinaryExpressionNode(operation, typeof(Boolean)) { Left = parameter, Right = parameter },
                Parameters = new List<ParameterExpressionNode> { parameter }
            };
            var expression = mapper.ToExpression(node);

            Assert.IsNotNull(expression);
        }

        static IExpressionNodeMapper<Func<Boolean, Boolean>> GetMapper() => new MapsterExpressionNodeMapper<Func<Boolean, Boolean>>();
    }

    [Test]
    public void Maps_Double_To_Double_Operations()
    {
        var operations = new[]
        {
            ExpressionType.Power, ExpressionType.PowerAssign
        };
        var mapper = GetMapper();
        var parameter = new ParameterExpressionNode(ExpressionType.Parameter, typeof(Double)) { Name = "t" };

        foreach (var operation in operations)
        {
            var node = new LambdaExpressionNode(ExpressionType.Lambda, typeof(Func<Double, Double>))
            {
                Body = new BinaryExpressionNode(operation, typeof(Double)) { Left = parameter, Right = parameter },
                Parameters = new List<ParameterExpressionNode> { parameter }
            };
            var expression = mapper.ToExpression(node);

            Assert.IsNotNull(expression);
        }

        static IExpressionNodeMapper<Func<Double, Double>> GetMapper() => new MapsterExpressionNodeMapper<Func<Double, Double>>();
    }

    [Test]
    public void Maps_Coalesce_Operation()
    {
        var mapper = GetMapper();
        var parameter = new ParameterExpressionNode(ExpressionType.Parameter, typeof(String)) { Name = "t" };
        var node = new LambdaExpressionNode(ExpressionType.Lambda, typeof(Func<String, String>))
        {
            Body = new BinaryExpressionNode(ExpressionType.Coalesce, typeof(String)) { Left = parameter, Right = parameter },
            Parameters = new List<ParameterExpressionNode> { parameter }
        };
        var expression = mapper.ToExpression(node);

        Assert.IsNotNull(expression);

        static IExpressionNodeMapper<Func<String, String>> GetMapper() => new MapsterExpressionNodeMapper<Func<String, String>>();
    }

    [Test]
    public void Throws_For_Unsupported_Operation()
    {
        var mapper = GetMapper();
        var parameter = new ParameterExpressionNode(ExpressionType.Parameter, typeof(String)) { Name = "t" };
        var node = new LambdaExpressionNode(ExpressionType.Lambda, typeof(Func<String, String>))
        {
            Body = new BinaryExpressionNode(ExpressionType.ArrayIndex, typeof(String)) { Left = parameter, Right = parameter },
            Parameters = new List<ParameterExpressionNode> { parameter }
        };

        Assert.Throws<NotSupportedException>(() => mapper.ToExpression(node));

        static IExpressionNodeMapper<Func<String, String>> GetMapper() => new MapsterExpressionNodeMapper<Func<String, String>>();
    }

    [Test]
    public void Maps_With_Multiple_Parameters()
    {
        var mapper = GetMapper();
        var parameters = new[]
        {
            new ParameterExpressionNode(ExpressionType.Parameter, typeof(Int32)) { Name = "t" },
            new ParameterExpressionNode(ExpressionType.Parameter, typeof(Int32)) { Name = "u" }
        };
        var node = new LambdaExpressionNode(ExpressionType.Lambda, typeof(Func<Int32, Int32, Int32>))
        {
            Body = new BinaryExpressionNode(ExpressionType.Add, typeof(Int32)) { Left = parameters[0], Right = parameters[1] },
            Parameters = parameters
        };

        Assert.IsNotNull(mapper.ToExpression(node));

        static IExpressionNodeMapper<Func<Int32, Int32, Int32>> GetMapper() => new MapsterExpressionNodeMapper<Func<Int32, Int32, Int32>>();
    }
}
