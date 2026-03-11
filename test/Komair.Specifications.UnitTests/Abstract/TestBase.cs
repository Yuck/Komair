using System.Linq.Expressions;
using Komair.Specifications.Abstract;

namespace Komair.Specifications.UnitTests.Abstract;

public abstract class TestBase
{
    protected const String LongString = "a long one";
    protected const String ShortString = "short";

    protected class ContainsLongSpecification : SpecificationBase<String>
    {
        public override Expression<Func<String, Boolean>> ToExpression() => t => t.Contains("long");
    }

    protected class ContainsOrtSpecification : SpecificationBase<String>
    {
        public override Expression<Func<String, Boolean>> ToExpression() => t => t.Contains("ort");
    }

    protected class EndsWithSpecification : SpecificationBase<String>
    {
        private readonly String _suffix;

        public EndsWithSpecification(String suffix)
        {
            _suffix = suffix;
        }

        public override Expression<Func<String, Boolean>> ToExpression() => t => t.EndsWith(_suffix);
    }

    protected class IsShortStringSpecification : SpecificationBase<String>
    {
        public override Expression<Func<String, Boolean>> ToExpression() => t => t.Length < 10;
    }

    protected class StartsWithSpecification(String prefix) : SpecificationBase<String>
    {
        public override Expression<Func<String, Boolean>> ToExpression() => t => t.StartsWith(prefix);
    }
}
