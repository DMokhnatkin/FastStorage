using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using FastStorage.Collection;
using FastStorage.Core;
using FastStorage.Expressions.EqualityComparer;
using JetBrains.Annotations;

namespace FastStorage.Expressions.OperationTargets
{
    /// <summary>
    /// All fields have one operation target (expression + root collection)
    /// </summary>
    internal class SimpleOperationTarget : IOperationTarget
    {
        [CanBeNull]
        public Expression Expression { get; }

        [NotNull]
        public IFastCollection RootFastCollection { get; }

        public Type TargetType => Expression != null ? Expression.Type : RootFastCollection.DataType;

        internal SimpleOperationTarget(Expression expression, [NotNull] IFastCollection rootFastCollection)
        {
            Expression = expression;
            RootFastCollection = rootFastCollection ?? throw new ArgumentNullException(nameof(rootFastCollection));
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            // TODO: use smart comparation
            if (!(obj is SimpleOperationTarget simpleOperationTarget))
                return false;
            return simpleOperationTarget.RootFastCollection == RootFastCollection &&
                   simpleOperationTarget.Expression.ToString() == Expression.ToString();
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            //TODO: use smart hash code
            return HashCodeHelper.CalcHashCode(RootFastCollection, Expression.ToString());
        }
    }

    /// <summary>
    /// Each field have its own operation target (expression + root collection)
    /// </summary>
    internal class CompoundOperationTarget
    {
        // TODO:
    }
    
//    /// <summary>
//    /// Wraps expression which is operation target.
//    /// </summary>
//    /// <remarks>
//    /// Operation target is some expression which can be covered by index.
//    /// </remarks>
//    internal class OperationTarget
//    {
//        /// <summary>
//        /// Operation target may depend on other operation targets
//        /// </summary>
//        /// <remarks> F.e. when we call <see cref="Queryable.Select"/> we will create new operation target which depends on select's source operation target</remarks>
//        public IReadOnlyCollection<OperationTarget> Dependencies { get; }
//
//        public Expression LocalExpression { get; }
//
//        private Expression _compiledExpression;
//        /// <summary>
//        /// Transforms operation target to real expression
//        /// </summary>
//        public Expression CompiledExpression => _compiledExpression ?? (_compiledExpression = CompileExpression());
//
//        public Type TargetType => ((LambdaExpression) LocalExpression).Body.Type;
//
//        public OperationTarget([NotNull] IEnumerable<OperationTarget> dependencies, Expression lambdaSelector)
//        {
//            if (dependencies == null) throw new ArgumentNullException(nameof(dependencies));
//            Dependencies = new ReadOnlyCollection<OperationTarget>(dependencies.ToList());
//            LocalExpression = EnsureIsValidSelector(lambdaSelector);
//            EnsureIsValidState();
//        }
//
//        private Expression CompileExpression()
//        {
//            _compiledExpression = new LambdaParametersReplacer().ReplaceLambdaParameters((LambdaExpression)LocalExpression, Dependencies.Select(x => x.CompiledExpression).ToArray());
//            return _compiledExpression;
//        }
//
//        private static Expression EnsureIsValidSelector(Expression selector)
//        {
//            if (selector == null)
//                return null;
//
//            if (selector.NodeType != ExpressionType.Lambda)
//                throw new ArgumentException("Only lambda selectors are available now.");
//
//            return selector;
//        }
//
//        private void EnsureIsValidState()
//        {
//            if (Dependencies.Count != (((LambdaExpression)LocalExpression)?.Parameters?.Count ?? 0))
//                throw new ArgumentException($"Count {nameof(Dependencies)} is not equal to count of {nameof(LocalExpression)} parameters.");
//        }
//
//        [NotNull]
//        public static OperationTarget CreateFromSelector<TIn, TOut>([NotNull] FastCollection<TIn> source, [NotNull] Expression<Func<TIn, TOut>> selector)
//        {
//            if (source == null) throw new ArgumentNullException(nameof(source));
//
//            return new OperationTarget(new []{ new QuerySourceOperationTarget() }, selector);
//        }
//
//        [NotNull]
//        public static OperationTarget CreateFromSelector([NotNull] IFastCollection source, [NotNull] Expression selector)
//        {
//            if (source == null) throw new ArgumentNullException(nameof(source));
//
//            return new OperationTarget(new[] { new QuerySourceOperationTarget() }, selector);
//        }
//    }
//
//    internal class QuerySourceOperationTarget : OperationTarget
//    {
//        public QuerySourceOperationTarget() : base(Enumerable.Empty<OperationTarget>(), null)
//        {
//        }
//    }
}
