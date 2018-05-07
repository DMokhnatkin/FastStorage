using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace FastStorage.Expressions.LinqVisitor
{
    internal class LinqExpressionVisitor : ExpressionVisitor
    {
        [NotNull]
        private readonly Dictionary<MethodInfo, Func<MethodCallExpression, Expression>> _methodCallMap;

        public LinqExpressionVisitor()
        {
            _methodCallMap = new Dictionary<MethodInfo, Func<MethodCallExpression, Expression>>
            {
                { LinqMethodsRegistry.Where, x => VisitWhere(new WhereExpressionWrapper(x)) },
                { LinqMethodsRegistry.Select, x => VisitSelect(new SelectExpressionWrapper(x)) },
                { LinqMethodsRegistry.SelectMany,  x => VisitSelectMany(new LinqExpressionWrapper(x, LinqMethodsRegistry.SelectMany)) },
                { LinqMethodsRegistry.Join,  x => VisitJoin(new LinqExpressionWrapper(x, LinqMethodsRegistry.Join)) },
                { LinqMethodsRegistry.GroupJoin,  x => VisitGroupJoin(new LinqExpressionWrapper(x, LinqMethodsRegistry.GroupJoin)) },
                { LinqMethodsRegistry.Zip,  x => VisitZip(new LinqExpressionWrapper(x, LinqMethodsRegistry.Zip)) }
            };
        }

        #region Overrides of ExpressionVisitor

        /// <inheritdoc />
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.IsGenericMethod && _methodCallMap.ContainsKey(node.Method.GetGenericMethodDefinition()))
            {
                return VisitLinqMethod(new LinqExpressionWrapper(node, node.Method.GetGenericMethodDefinition()));
            }

            return base.VisitMethodCall(node);
        }

        #endregion

        protected virtual Expression VisitLinqMethod(LinqExpressionWrapper node)
        {
            return _methodCallMap[node.BaseLinqMethod](node.InnerExpression);
        }

        protected virtual Expression VisitSelect(SelectExpressionWrapper node)
        {
            return base.VisitMethodCall(node.InnerExpression);
        }

        protected virtual Expression VisitWhere(WhereExpressionWrapper node)
        {
            return base.VisitMethodCall(node.InnerExpression);
        }

        protected virtual Expression VisitSelectMany(LinqExpressionWrapper node)
        {
            return base.VisitMethodCall(node.InnerExpression);
        }

        protected virtual Expression VisitJoin(LinqExpressionWrapper node)
        {
            return base.VisitMethodCall(node.InnerExpression);
        }

        protected virtual Expression VisitGroupJoin(LinqExpressionWrapper node)
        {
            return base.VisitMethodCall(node.InnerExpression);
        }

        protected virtual Expression VisitZip(LinqExpressionWrapper node)
        {
            return base.VisitMethodCall(node.InnerExpression);
        }
    }
}
