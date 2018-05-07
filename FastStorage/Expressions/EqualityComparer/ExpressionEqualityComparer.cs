using System.Collections.Generic;

namespace FastStorage.Expressions.EqualityComparer
{
    /// <summary>
    /// Compares two expression trees for equality
    /// </summary>
    internal class ExpressionEqualityComparer : IEqualityComparer<ComparableExpression>
    {
        /// <inheritdoc />
        public bool Equals(ComparableExpression x, ComparableExpression y)
        {
            if (x == null || y == null)
                return false;
            // TODO: compare not just hash codes?
            return x.GetHashCode() == y.GetHashCode();
        }

        /// <inheritdoc />
        public int GetHashCode(ComparableExpression obj)
        {
            if (obj == null)
                return -1;
            return obj.GetHashCode();
        }
    }
}
