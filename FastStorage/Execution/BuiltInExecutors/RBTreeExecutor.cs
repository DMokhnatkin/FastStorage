using System;
using System.Linq;
using System.Linq.Expressions;
using FastStorage.Core;
using FastStorage.Execution.ExecutorsCore;
using FastStorage.Execution.ExecutorsCore.Complexity;
using FastStorage.Expressions.Filters;
using FastStorage.Expressions.OperationTargets;
using FastStorage.Indices;
using JetBrains.Annotations;

namespace FastStorage.Execution.BuiltInExecutors
{
    internal class RBTreeExecutor<T> : IOperationExecutor
    {
        [NotNull]
        private readonly RedBlackTreeIndex<T> _redBlackTreeIndex;

        public RBTreeExecutor(RedBlackTreeIndex<T> redBlackTreeIndex)
        {
            _redBlackTreeIndex = redBlackTreeIndex ?? throw new ArgumentNullException(nameof(redBlackTreeIndex));
        }
        
        /// <inheritdoc />
        public PreparedExecution PrepareExecution(IOperation operation)
        {
            if (operation is ComparisonFilter baseFilter)
                return PrepareBaseFilterExecution(baseFilter);

            return PreparedExecution.Unsuccess;
        }

        [NotNull]
        private static PreparedExecution RedBlackTreePreparedExecution(Func<int[]> executionAction)
        {
            return new PreparedExecution(true, ComplexityConstants.Logarithmic, executionAction);
        }
        
        [NotNull]
        private PreparedExecution PrepareBaseFilterExecution(ComparisonFilter comparisonFilter)
        {
            if (!(comparisonFilter.Value is T constValue)) return PreparedExecution.Unsuccess;
            
            switch (comparisonFilter.ComparisonFilterType)
            {
                case ComparisonFilterType.Greater:
                    return RedBlackTreePreparedExecution(() => _redBlackTreeIndex.DataStruct.GetGreater(constValue, false).ToArray());
                case ComparisonFilterType.GreaterOrEqual:
                    return RedBlackTreePreparedExecution(() => _redBlackTreeIndex.DataStruct.GetGreater(constValue, true).ToArray());
                case ComparisonFilterType.Less:
                    return RedBlackTreePreparedExecution(() => _redBlackTreeIndex.DataStruct.GetLess(constValue, false).ToArray());
                case ComparisonFilterType.LessOrEqual:
                    return RedBlackTreePreparedExecution(() => _redBlackTreeIndex.DataStruct.GetLess(constValue, true).ToArray());
                case ComparisonFilterType.Equal:
                    return RedBlackTreePreparedExecution(() => _redBlackTreeIndex.DataStruct.Get(constValue).ToArray());
            }

            return PreparedExecution.Unsuccess;
        }
    }
}