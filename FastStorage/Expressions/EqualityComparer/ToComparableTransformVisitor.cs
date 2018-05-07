using System.Linq.Expressions;

namespace FastStorage.Expressions.EqualityComparer
{
    /// <summary>
    /// It is expression tree visitor which transforms expression tree to comparable form.
    /// </summary>
    internal class ToComparableTransformVisitor : ExpressionVisitor
    {
        // TODO: implement smart logic 
        // 1. Order params in determenic order (where we can do it)
        // 2. Rename params and other stuff to determenic names (like "name1", "name2" ...)

        public ComparableExpression TransformToComparable(Expression expression)
        {
            return new ComparableExpression(Visit(expression));
        }
    }
}
