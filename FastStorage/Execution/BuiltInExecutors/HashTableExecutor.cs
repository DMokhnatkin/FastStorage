using System;
using System.Linq;
using FastStorage.Core;
using FastStorage.Execution.ExecutorsCore;
using FastStorage.Execution.ExecutorsCore.Complexity;
using FastStorage.Expressions.Filters;
using FastStorage.Indices;
using JetBrains.Annotations;

namespace FastStorage.Execution.BuiltInExecutors
{
    internal class HashTableExecutor<T> : IOperationExecutor
    {
        private readonly HashTableIndex<T> _hashTableIndex;

        public HashTableExecutor(HashTableIndex<T> hashTableIndex)
        {
            _hashTableIndex = hashTableIndex;
        }

        /// <inheritdoc />
        public PreparedExecution PrepareExecution(IOperation operation)
        {
            if (operation is ComparisonFilter baseFilter)
                return PrepareBaseFilterExecution(baseFilter);

            return PreparedExecution.Unsuccess;
        }
        
        [NotNull]
        private PreparedExecution PrepareBaseFilterExecution(ComparisonFilter comparisonFilter)
        {
            if (!(comparisonFilter.Value is T constValue)) return PreparedExecution.Unsuccess;
            
            switch (comparisonFilter.ComparisonFilterType)
            {
                case ComparisonFilterType.Equal:
                    return RedBlackTreePreparedExecution(() => _hashTableIndex.Get(constValue).ToArray());
            }

            return PreparedExecution.Unsuccess;
        }
        
        [NotNull]
        private static PreparedExecution RedBlackTreePreparedExecution(Func<int[]> executionAction)
        {
            return new PreparedExecution(true, ComplexityConstants.Constant, executionAction);
        }
    }
}