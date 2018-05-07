using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace FastStorage.Expressions.Filters
{
    /// <summary>
    /// Expression covered by filters
    /// </summary>
    /// <remarks>To cover expression with filters <see cref="BaseFiltersProcessor"/></remarks>
    internal class FiltersCoveredExpression
    {
        public Expression Expression { get; }

        [NotNull]
        private readonly Dictionary<Expression, ComparisonFilter> _filters;

        public FiltersCoveredExpression([NotNull] Dictionary<Expression, ComparisonFilter> filters, [NotNull] Expression expression)
        {
            _filters = filters ?? throw new ArgumentNullException(nameof(filters));
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public ComparisonFilter GetFilter(Expression expression)
        {
            if (!_filters.ContainsKey(expression))
                return null;
            return _filters[expression];
        }
    }
}
