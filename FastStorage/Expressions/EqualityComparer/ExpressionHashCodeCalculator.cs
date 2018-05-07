using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace FastStorage.Expressions.EqualityComparer
{
    /// <summary>
    /// Calculates hash code of expression tree
    /// </summary>
    internal class ExpressionHashCodeCalculator
    {
        public int CalculateHashCode(Expression expression)
        {
            var calculator = new ExpressionHashCodeCalcVisitor();
            return calculator.CalculateHashCode(expression);
        }

        /// <summary>
        /// It is expression visitor, it can calculate hash code of expression tree.
        /// This class is inner, because i want exclude Visit method from ExpressionHashCodeCalculator
        /// </summary>
        private class ExpressionHashCodeCalcVisitor : ExpressionVisitor
        {
            private const int NullHashCode = 68972971;

            /// <summary>
            /// Compare 2 expressions by reference equality
            /// </summary>
            private class ExpressionComparer : IEqualityComparer<Expression>
            {
                /// <inheritdoc />
                public bool Equals(Expression x, Expression y)
                {
                    return Object.ReferenceEquals(x, y);
                }

                /// <inheritdoc />
                public int GetHashCode(Expression obj)
                {
                    return RuntimeHelpers.GetHashCode(obj);
                }
            }

            [NotNull]
            private readonly Dictionary<Expression, int> _hashCodes = new Dictionary<Expression, int>(new ExpressionComparer());

            public int CalculateHashCode(Expression expression)
            {
                _hashCodes.Clear();
                var expr = Visit(expression);
                return _hashCodes[expr];
            }

            private void HandleExpressionProperty(Expression expression, List<object> valuesForCalcHashCode)
            {
                if (!_hashCodes.ContainsKey(expression))
                    throw new IndexOutOfRangeException($"Hash code for {expression} was not calculated before it was used.");
                valuesForCalcHashCode.Add(_hashCodes[expression]);
            }

            /// <summary>
            /// Using reflection to calculate hash code of expression (hash code for all related expressions must be calculated before (such as expressions for arguments))
            /// </summary>
            private int AutoCalculateHashCode(Expression expression)
            {
                if (expression == null) return NullHashCode;

                var valuesForCalcHashCode = new List<object>();
                foreach (var propInfo in expression.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance))
                {
                    var val = propInfo.GetValue(expression);
                    if (val == null)
                    {
                        valuesForCalcHashCode.Add(NullHashCode);
                        continue;
                    }
                    if (val is Expression expressionPropertyValue)
                    {
                        HandleExpressionProperty(expressionPropertyValue, valuesForCalcHashCode);
                        continue;
                    }
                    if (val is IEnumerable<Expression> expressionsPropertyValue)
                    {
                        foreach (var expr in expressionsPropertyValue)
                        {
                            HandleExpressionProperty(expr, valuesForCalcHashCode);
                        }
                        continue;
                    }
                    valuesForCalcHashCode.Add(val);
                }

                return HashCodeHelper.CalcHashCode(valuesForCalcHashCode.ToArray());
            }

            #region Overrides of ExpressionVisitor

            /// <inheritdoc />
            public override Expression Visit(Expression node)
            {
                var res = base.Visit(node);
                if (res == null)
                    return res;
                _hashCodes[res] = AutoCalculateHashCode(res);
                return res;
            }

            #endregion
        }
    }
}
