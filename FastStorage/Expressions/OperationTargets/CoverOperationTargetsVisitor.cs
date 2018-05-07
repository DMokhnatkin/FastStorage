//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;
//using FastStorage.Core;
//using FastStorage.Expressions.LinqVisitor;
//using JetBrains.Annotations;
//
//namespace FastStorage.Expressions.OperationTargets
//{
//    internal class CoverOperationTargetsVisitor : LinqExpressionVisitor
//    {
//        // This dictionary will store operation target for each expression in expression tree
//        [NotNull]
//        private readonly Dictionary<Expression, SimpleOperationTarget> _operationTargetsCoverage = new Dictionary<Expression, SimpleOperationTarget>();
//
//        public TargetsCoveredExpression CoverExpressionWithOperationTargets(Expression expression)
//        {
//            _operationTargetsCoverage.Clear();
//            Visit(expression);
//            return new TargetsCoveredExpression(_operationTargetsCoverage, expression);
//        }
//
//        #region Overrides of LinqExpressionVisitor
//
//        /// <inheritdoc />
//        protected override Expression VisitConstant(ConstantExpression node)
//        {
//            var t = base.VisitConstant(node);
//            if (node.Type.IsConstructedGenericType && node.Type.GetGenericTypeDefinition() == typeof(FastCollection<>))
//            {
//                if (!_operationTargetsCoverage.ContainsKey(node))
//                {
//                    if (!(node.Value is IFastCollection fastCollection))
//                        throw new InvalidOperationException("Can't get fast collection id");
//                    _operationTargetsCoverage[node] = new QuerySourceOperationTarget(fastCollection.FastCollectionId);
//                }
//            }
//            return t;
//        }
//
//        /// <inheritdoc />
//        protected override Expression VisitLinqMethod(LinqExpressionWrapper node)
//        {
//            var t = base.VisitLinqMethod(node);
//            // If this linq expression didn't create new operation target after VisitLinqMethod, 
//            // we will set same as source operation target for this expression node. 
//            if (!_operationTargetsCoverage.ContainsKey(node.InnerExpression))
//            {
//                _operationTargetsCoverage[node.InnerExpression] = _operationTargetsCoverage[node.Source];
//            }
//            return t;
//        }
//
//        /// <inheritdoc />
//        protected override Expression VisitSelect(SelectExpressionWrapper node)
//        {
//            var res = base.VisitSelect(node);
//            // Create new operation target
//            _operationTargetsCoverage[node.InnerExpression] = _operationTargetsCoverage[node.Source].ExtendWithSelector(node.Selector);
//            return res;
//        }
//
//        #endregion
//    }
//}
