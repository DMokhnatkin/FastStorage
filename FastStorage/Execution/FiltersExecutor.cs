using System;
using System.Collections.Generic;
using System.Linq;
using FastStorage.Collection;
using FastStorage.Core;
using FastStorage.Execution.ExecutorsCore;
using FastStorage.Expressions.Filters;
using JetBrains.Annotations;

namespace FastStorage.Execution
{
    internal class FiltersExecutor
    {
        /// <summary>
        /// This method will handle all work for prepare filter execution.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="indicesCoverage"></param>
        /// <returns></returns>
        [NotNull]
        public PreparedExecution PrepareExecution(IFilter filter, IIndicesCoverage indicesCoverage)
        {
            var executionPipeline = FiltersExecutionPipeline.Build(filter);
            var availableExecutorsColelction = ExecutorsManager.Instance.AvailableExecutorsCollection;
            // In this collection we will collect best executors of each filter in tree
            var allPreparedFilterExecutions = new List<Tuple<IFilter, PreparedExecution>>();
            foreach (var executionGroup in executionPipeline.GetExecutionGroups())
            {
                foreach (var localFilter in executionGroup.Filters)
                {
                    if (!(localFilter is IOneOperationTargetFilter oneOperationTargetFilter))
                        return PreparedExecution.Unsuccess; 
                    
                    // Get available for filter's operation target indices.
                    var availableIndices = indicesCoverage.GetIndices(oneOperationTargetFilter.OperationTarget);
                    // Find all executors which are allowed with specifed indices 
                    var allowedExecutors = availableExecutorsColelction.GetAvailableExecutors(oneOperationTargetFilter.OperationTarget.TargetType, availableIndices);
                    // Prepare execution with all of them
                    var preparedExecutions = allowedExecutors.Select(x => x.PrepareExecution(localFilter)).ToArray();
                    // Find executor which can execute this filter with minimal complexity 
                    var bestPreparedExecution = preparedExecutions
                        .Where(x => x.CanBeExecuted)
                        .OrderBy(x => x.TimeComplexity.Weight)
                        .FirstOrDefault();
                    if (bestPreparedExecution == null && !(localFilter is CompoundFilter))
                        // TODO: if we can't execute one filter in tree we don't need to break execution for whole tree
                        return PreparedExecution.Unsuccess;
                    allPreparedFilterExecutions.Add(new Tuple<IFilter, PreparedExecution>(localFilter, bestPreparedExecution));
                }
            }

            // TODO: parallel filters execution
            return CombinePreparedExecutions(allPreparedFilterExecutions);
        }

        /// <summary>
        /// Combines all prepared execution in one prepared execution
        /// </summary>
        /// <param name="preparedFilterExecutions"></param>
        /// <returns></returns>
        [NotNull]
        private PreparedExecution CombinePreparedExecutions([NotNull] List<Tuple<IFilter, PreparedExecution>> preparedFilterExecutions)
        {
            var maxComplexity = preparedFilterExecutions
                .OrderByDescending(x => x.Item2.TimeComplexity.Weight)
                .Select(x => x.Item2.TimeComplexity)
                .FirstOrDefault();
            return new PreparedExecution(true, maxComplexity, () =>
            {
                var executionResults = new Dictionary<IFilter, int[]>(ObjectReferenceEqualityComparer<IFilter>.Default);
                foreach (var preparedFilterExecution in preparedFilterExecutions)
                {
                    // TODO: create compoundFilter executor and handle it as other filters 
                    if (preparedFilterExecution.Item2 == null &&
                        preparedFilterExecution.Item1 is CompoundFilter compoundFilter)
                    {
                        switch (compoundFilter.OperationType)
                        {
                            case CompoundFilter.CompoundFilterType.And:
                                var operands = compoundFilter.Operands.ToArray();
                                var res = new HashSet<int>(executionResults[operands[0]]);
                                for (int i = 1; i < res.Count; i++)
                                {
                                    res.IntersectWith(executionResults[operands[i]]);
                                }
                                executionResults.Add(preparedFilterExecution.Item1, res.ToArray());
                                continue;
                            case CompoundFilter.CompoundFilterType.Or:
                                var operands2 = compoundFilter.Operands.ToArray();
                                var res2 = new HashSet<int>();
                                for (int i = 0; i < res2.Count; i++)
                                {
                                    res2.UnionWith(executionResults[operands2[i]]);
                                }
                                executionResults.Add(preparedFilterExecution.Item1, res2.ToArray());
                                continue;
                            default:
                                throw new ArgumentException($"Filters executor dosn't know how to execute Compoubd filter with {compoundFilter.OperationType} operation type");
                        }
                    }
                    executionResults.Add(preparedFilterExecution.Item1, preparedFilterExecution.Item2.ExecuteAction());
                }

                return executionResults[preparedFilterExecutions.Last().Item1];
            });
        }
    }
}