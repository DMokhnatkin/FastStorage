using System.Linq.Expressions;

namespace FastStorage.Expressions.EqualityComparer
{
    internal static class CompareExpressionsExtensions
    {
        public static ComparableExpression ToComparable(this Expression expression)
        {
            var toComparableTransformer = new ToComparableTransformVisitor();
            return toComparableTransformer.TransformToComparable(expression);
        }

        public static bool AreEqual(Expression expression1, Expression expression2)
        {
            var comparable1 = expression1.ToComparable();
            var comparable2 = expression2.ToComparable();
            return comparable1.Equals(comparable2);
        }
    }
}
