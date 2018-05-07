using Expression = System.Linq.Expressions.Expression;

namespace FastStorage.Expressions.EqualityComparer
{
    /// <summary>
    /// This class just wraps expression tree for compile time type check 
    /// (when tree is transformed to comparable form and where it is not)
    /// </summary>
    internal class ComparableExpression
    {
        public Expression Expression { get; }

        public ComparableExpression(Expression expression)
        {
            Expression = expression;
            var hashCodeCalculator = new ExpressionHashCodeCalculator();
            _hashCode = hashCodeCalculator.CalculateHashCode(expression);
        }

        private readonly int _hashCode;

        /// <inheritdoc />
        public override int GetHashCode() => _hashCode;

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is ComparableExpression castedObj)
            {
                var comparer = new ExpressionEqualityComparer();
                return comparer.Equals(this, castedObj);
            }

            return false;
        }

        public static bool operator ==(ComparableExpression expr1, ComparableExpression expr2)
        {
            return Equals(expr1, expr2);
        }

        public static bool operator !=(ComparableExpression expr1, ComparableExpression expr2)
        {
            return !(expr1 == expr2);
        }
    }
}
