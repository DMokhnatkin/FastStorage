using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace FastStorage.Expressions.Helpers
{
    /// <summary>
    /// This visitor can replace lambda parameters in expression tree with some specifed expressions. 
    /// </summary>
    internal partial class LambdaParametersReplacer
    {
        /// <summary>
        /// Returns copy of body of specifed expression with replaced lambda parameters.
        /// </summary>
        public static Expression ReplaceLambdaParameters([NotNull] LambdaExpression expression, [NotNull] params Expression[] parameters)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            if (expression.Parameters.Count != parameters.Length)
                throw new ArgumentException($"Invalid count of {nameof(parameters)}. Expected count was {nameof(expression)} (same as lambda expression parameters count)");

            var parametersArray = parameters.ToArray();
            var res = new Dictionary<ParameterExpression, Expression>();
            for (int i = 0; i < parameters.Length; i++)
            {
                res.Add(expression.Parameters[i], parametersArray[i]);
            }

            return new ReplaceLambdaParameterExpressionVisitor(res).Visit(expression.Body);
        } 
    }

    internal partial class LambdaParametersReplacer
    {
        private class ReplaceLambdaParameterExpressionVisitor : ExpressionVisitor
        {
            private IDictionary<ParameterExpression, Expression> _expressionsToReplace;

            public ReplaceLambdaParameterExpressionVisitor(IDictionary<ParameterExpression, Expression> expressionsToReplace)
            {
                _expressionsToReplace = expressionsToReplace ?? throw new ArgumentNullException(nameof(expressionsToReplace));
            }

            /// <inheritdoc />
            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (_expressionsToReplace.ContainsKey(node))
                    return _expressionsToReplace[node];
                return base.VisitParameter(node);
            }
        }
    }
}
