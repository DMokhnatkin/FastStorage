using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace FastStorage.Expressions.Filters
{
    /// <summary>
    /// Visit expression and parses and cover it with filters.
    /// </summary>
    internal class BaseFiltersProcessor
    {
        [NotNull]
        public FiltersCoveredExpression CoverWithFilters([NotNull] Expression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            var visitor = new CoverWithFiltersVisitor();
            return new FiltersCoveredExpression(visitor.Cover(expression), expression);
        }

        private class CoverWithFiltersVisitor : ExpressionVisitor
        {
            [NotNull]
            private readonly BaseFiltersParser _baseFiltersParser = new BaseFiltersParser();

            [NotNull]
            private Dictionary<Expression, ComparisonFilter> _filtersCoverageData = new Dictionary<Expression, ComparisonFilter>();

            [NotNull]
            public Dictionary<Expression, ComparisonFilter> Cover([NotNull] Expression expression)
            {
                _filtersCoverageData = new Dictionary<Expression, ComparisonFilter>();

                Visit(expression);

                return _filtersCoverageData;
            }

            /// <inheritdoc />
            public override Expression Visit(Expression node)
            {
                var t = _baseFiltersParser.TryParseBaseFilter(node);
                if (t != null)
                {
                    _filtersCoverageData[node] = t;
                    return node;
                }
                return base.Visit(node);
            }
        }
    }
}
