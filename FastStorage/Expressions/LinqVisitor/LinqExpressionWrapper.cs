using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace FastStorage.Expressions.LinqVisitor
{
    internal class LinqExpressionWrapper
    {
        /// <summary>
        /// Full method call expression
        /// </summary>
        [NotNull]
        public MethodCallExpression InnerExpression { get; }

        /// <summary>
        /// For this expression linq method was applied (usually is method argument marked as "this")
        /// </summary>
        public Expression Source { get; }

        /// <summary>
        /// Type of concrete linq method
        /// </summary>
        public MethodInfo BaseLinqMethod { get; }

        public LinqExpressionWrapper(MethodCallExpression innerExpression, MethodInfo baseLinqMethod)
        {
            InnerExpression = innerExpression ?? throw new ArgumentNullException(nameof(innerExpression));
            BaseLinqMethod = baseLinqMethod ?? throw new ArgumentNullException(nameof(baseLinqMethod));
            if (innerExpression.Arguments.Count == 0)
                throw new ArgumentException($"Invalid number of arguments for linq expression (it must be 1 and more, but {innerExpression.Arguments.Count} was)");
            Source = innerExpression.Arguments[0];
        }
    }

    internal class SelectExpressionWrapper : LinqExpressionWrapper
    {
        public Expression Selector { get; }

        public SelectExpressionWrapper(MethodCallExpression innerExpression) : base(innerExpression, LinqMethodsRegistry.Select)
        {
            if (innerExpression.Arguments.Count != 2)
                throw new ArgumentException($"Invalid number of arguments for linq select expression (it must be 2, but {innerExpression.Arguments.Count} was)");
            Selector = InnerExpression.Arguments[1];
        }
    }

    internal class WhereExpressionWrapper : LinqExpressionWrapper
    {
        public Expression Predicate { get; }

        /// <inheritdoc />
        public WhereExpressionWrapper(MethodCallExpression innerExpression) : base(innerExpression, LinqMethodsRegistry.Where)
        {
            if (innerExpression.Arguments.Count != 2)
                throw new ArgumentException($"Invalid number of arguments for linq where expression (it must be 2, but {innerExpression.Arguments.Count} was)");
            Predicate = InnerExpression.Arguments[1];
        }
    }
}
