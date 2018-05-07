using System;
using System.Linq.Expressions;
using FastStorage.Core;
using FastStorage.Expressions.Helpers;
using JetBrains.Annotations;

namespace FastStorage.Expressions.OperationTargets
{
    internal static class OperationTargetBuilder
    {
        public static SimpleOperationTarget ExtendWithSelector<TSource, TResult>(this SimpleOperationTarget sourceTarget,
            Expression<Func<TSource, TResult>> selector)
        {
            var normalSelector = new SelectorTransformer().TransformToNormalForm(selector);
            var t = LambdaParametersReplacer.ReplaceLambdaParameters((LambdaExpression) normalSelector, sourceTarget.Expression);
            return new SimpleOperationTarget(t, sourceTarget.RootFastCollection);
        }
        
        /// <summary>
        /// Build operation target for fast collection.
        /// </summary>
        /// <param name="rootFastCollection"></param>
        /// <param name="selector"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        [NotNull]
        public static SimpleOperationTarget BuildForRoot<TSource>(
            [NotNull] IFastCollection<TSource> rootFastCollection)
        {
            return new SimpleOperationTarget(new FastCollectionReferenceExpression(rootFastCollection), rootFastCollection);
        }
        
        /// <summary>
        /// Build operation target for specifed fast collection and selector.
        /// </summary>
        /// <param name="rootFastCollection"></param>
        /// <param name="selector"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        [NotNull]
        public static SimpleOperationTarget BuildForSelector<TSource, TResult>(
            [NotNull] IFastCollection<TSource> rootFastCollection, 
            [NotNull] Expression<Func<TSource, TResult>> selector)
        {
            var normalSelector = new SelectorTransformer().TransformToNormalForm(selector);
            var t = LambdaParametersReplacer.ReplaceLambdaParameters((LambdaExpression) normalSelector, new FastCollectionReferenceExpression(rootFastCollection));
            return new SimpleOperationTarget(t, rootFastCollection);
        }

        /// <summary>
        /// Build operation target for some expression tree.
        /// </summary>
        /// <param name="expression">
        /// For this expression operation target will be built.
        /// Expression tree must contains one and only one constant with <see cref="IFastCollection"/> value.
        /// </param>
        /// <returns></returns>
        [NotNull]
        public static SimpleOperationTarget Build(Expression expression)
        {
            IFastCollection rootFastCollection = null;
            var operationTargetExpression = ExpressionNodeReplacer.ReplaceNode(expression, expr =>
            {
                if (expr != null &&
                    expr is FastCollectionReferenceExpression fastCollectionReferenceExpression)
                {
                    if (rootFastCollection != null) 
                        throw new ArgumentException("Multiple fast collections were found in expression tree when build operation target.");
                    rootFastCollection = fastCollectionReferenceExpression.FastCollection;
                    return expr;
                }

                return null;
            });
            if (rootFastCollection == null) 
                throw new ArgumentException($"No {nameof(FastCollectionReferenceExpression)} were found in expression tree when build operation target.");
            return new SimpleOperationTarget(operationTargetExpression, rootFastCollection);
        }
    }
}