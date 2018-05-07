using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace FastStorage.Expressions.OperationTargets
{
    internal class TargetsCoveredExpression
    {
        [NotNull]
        public Expression Expression { get; }

        [NotNull]
        private readonly Dictionary<Expression, SimpleOperationTarget> _operationTargets;

        public TargetsCoveredExpression([NotNull] Dictionary<Expression, SimpleOperationTarget> operationTargets, [NotNull] Expression expression)
        {
            _operationTargets = operationTargets ?? throw new ArgumentNullException(nameof(operationTargets));
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public SimpleOperationTarget GetOperationTarget(Expression expression)
        {
            if (!_operationTargets.ContainsKey(expression))
                return null;
            return _operationTargets[expression];
        }
    }
}
