using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace FastStorage.Expressions.Helpers
{
    /// <summary>
    /// Replaces specifed node.
    /// </summary>
    public static class ExpressionNodeReplacer
    {
        /// <summary>
        /// This method will walk over source expression tree and call replacer for each node.
        /// When replacer returns not null value, node will be replaced with this value.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="replacer"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Expression ReplaceNode([NotNull] Expression source, [NotNull] Func<Expression, Expression> replacer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (replacer == null) throw new ArgumentNullException(nameof(replacer));
            
            var visitor = new ReplaceNodeVisitor(replacer);
            return visitor.Visit(source);
        }
        
        private class ReplaceNodeVisitor : ExpressionVisitor
        {
            [NotNull]
            private readonly Func<Expression, Expression> _replacer;

            public ReplaceNodeVisitor([NotNull] Func<Expression, Expression> replacer)
            {
                _replacer = replacer;
            }

            /// <inheritdoc />
            public override Expression Visit(Expression node)
            {
                var replaceValue = _replacer(node);
                if (replaceValue != null)
                    return replaceValue;
                
                return base.Visit(node);
            }
        }
    }
}