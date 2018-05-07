using System;
using System.Linq.Expressions;
using FastStorage.Expressions.OperationTargets;
using JetBrains.Annotations;

namespace FastStorage.Expressions.Filters
{
    internal class BaseFiltersParser
    {
        private readonly Func<Expression, ComparisonFilter>[] _parsePatterns = {
            expr => TryParseComparisonFilter(expr),
            expr => TryParseLinq(expr)
        };

        [CanBeNull]
        public ComparisonFilter TryParseBaseFilter(Expression expression)
        {
            foreach (var pattern in _parsePatterns)
            {
                var t = pattern.Invoke(expression);
                if (t != null)
                    return t;
            }

            return null;
        }

        /// <summary>
        /// Try parse comparasion filter (less, greater and others)
        /// </summary>
        private static ComparisonFilter TryParseComparisonFilter(Expression expression)
        {
            if (!(expression is BinaryExpression binaryExpression))
                return null;

            var comparationType = ComparisonFilterType.None;
            switch (expression.NodeType)
            {
                case ExpressionType.GreaterThan:
                    comparationType = ComparisonFilterType.Greater;
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    comparationType = ComparisonFilterType.GreaterOrEqual;
                    break;
                case ExpressionType.LessThan:
                    comparationType = ComparisonFilterType.Less;
                    break;
                case ExpressionType.LessThanOrEqual:
                    comparationType = ComparisonFilterType.LessOrEqual;
                    break;
                case ExpressionType.Equal:
                    comparationType = ComparisonFilterType.Equal;
                    break;
                case ExpressionType.NotEqual:
                    comparationType = ComparisonFilterType.NotEqual;
                    break;
                default:
                    return null;
            }

            if (binaryExpression.Right.NodeType == ExpressionType.Constant)
            {
                var operationTarget = OperationTargetBuilder.Build(binaryExpression.Left);
                if (operationTarget == null) return null;
                return new ComparisonFilter(operationTarget, comparationType, ((ConstantExpression)binaryExpression.Right).Value);
            }
            
            if (binaryExpression.Left.NodeType == ExpressionType.Constant)
            {
                var operationTarget = OperationTargetBuilder.Build(binaryExpression.Right);
                if (operationTarget == null) return null;
                return new ComparisonFilter(operationTarget, comparationType.Mirror(), ((ConstantExpression)binaryExpression.Left).Value);
            }

            return null;
        }

        /// <summary>
        /// Try parse compound filter
        /// </summary>
        public CompoundFilter TryParseCompoundFilter(Expression expression, IFilter leftChild, IFilter rightChild)
        {
            CompoundFilter.CompoundFilterType comparationType;
            switch (expression.NodeType)
            {
                case ExpressionType.AndAlso:
                    comparationType = CompoundFilter.CompoundFilterType.And;
                    break;
                case ExpressionType.OrElse:
                    comparationType = CompoundFilter.CompoundFilterType.Or;
                    break;
                default:
                    return null;
            }
            
            return new CompoundFilter(comparationType, leftChild, rightChild);
        }


        /// <summary>
        /// Try parse linq filter operations (Contains, Any and other)
        /// </summary>
        private static ComparisonFilter TryParseLinq(Expression expression)
        {
            // TODO:
            return null;
        }
    }
}
