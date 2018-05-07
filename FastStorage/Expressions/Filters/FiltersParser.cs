using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FastStorage.Expressions.Helpers;
using FastStorage.Expressions.OperationTargets;
using JetBrains.Annotations;

namespace FastStorage.Expressions.Filters
{
    /// <summary>
    /// This class contains logic on filters parsing
    /// </summary>
    internal class FiltersParser
    {
        [NotNull]
        private readonly FiltersParserVisitor _filtersParser = new FiltersParserVisitor();

        /// <summary>
        /// Transforms "where" predicate to inner presentation (which is used for execution).
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="sourceOperationTarget">Lambda parameter of expression will be replaced with <see cref="OperationTargetExpression"/> containing this target</param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        internal IFilter ParsePredicate<TSource>([NotNull] Expression<Func<TSource, bool>> expression, SimpleOperationTarget sourceOperationTarget)
        {
            var t = LambdaParametersReplacer.ReplaceLambdaParameters(expression, sourceOperationTarget.Expression);
            return _filtersParser.ParseFilters(t);
        }

        class FiltersParserVisitor : ExpressionVisitor
        {
            [NotNull]
            private readonly BaseFiltersParser _baseFiltersParser = new BaseFiltersParser();
            
            // We can't ovveride reurn type of visit methods, so lets save founded filters in dictionary
            [NotNull]
            private readonly Dictionary<Expression, IFilter> _foundedFilters = new Dictionary<Expression, IFilter>();

            internal IFilter ParseFilters(Expression expression)
            {
                _foundedFilters.Clear();
                Visit(expression);
                if (!_foundedFilters.ContainsKey(expression))
                    throw new InvalidOperationException($"Error when parsing filters for {expression}. " +
                                                        $"It can happen if parser is not smart enough or if expression is not valid filter.");
                var res = _foundedFilters[expression];
                _foundedFilters.Clear();
                return res;
            }

            /// <inheritdoc />
            public override Expression Visit(Expression node)
            {
                // Recursive immersion. We need to parse base filters here (like as Comparation)
                var t = _baseFiltersParser.TryParseBaseFilter(node);
                if (t != null)
                {
                    _foundedFilters[node] = t;
                    // There is no need to check this branch further
                    return node;
                }

                var res = base.Visit(node);

                // Recursive ascent. We need to parse compound filters here (in recursive immersion we have parsed all required base filters)
                if (!_foundedFilters.ContainsKey(node))
                {
                    if (node is BinaryExpression binaryExpression)
                    {
                        if (!_foundedFilters.ContainsKey(binaryExpression.Left) || !_foundedFilters.ContainsKey(binaryExpression.Right))
                            throw new InvalidOperationException($"Error when parsing filters. " +
                                                                $"It can happen if parser is not smart enough or if expression is not valid filter.");
                        var compoundFilter = _baseFiltersParser.TryParseCompoundFilter(binaryExpression,
                            _foundedFilters[binaryExpression.Left], _foundedFilters[binaryExpression.Right]);
                        if (compoundFilter != null)
                        {
                            _foundedFilters[node] = compoundFilter;
                            // There is no need to check this branch further
                            return node;
                        }
                    }
                }

                return res;
            }
        }
    }
}