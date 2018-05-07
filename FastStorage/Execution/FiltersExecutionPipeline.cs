using System;
using System.Collections.Generic;
using System.Linq;
using FastStorage.Expressions.Filters;
using JetBrains.Annotations;

namespace FastStorage.Execution
{
    /// <summary>
    /// Some filters are very complex. F.e. when we need to filter by 2 fields and than combine result we
    /// will create one <see cref="CompoundFilter"/> which relies on two <see cref="ComparisonFilter"/>.
    /// When we will execute this filter, we will need to do it in right order (1-st step execute 2 <see cref="ComparisonFilter"/> (maybe parallel) and only than execute <see cref="CompoundFilter"/>).
    /// This class contains logic about this order.
    /// </summary>
    internal class FiltersExecutionPipeline
    {
        [NotNull] readonly Dictionary<IFilter, int> _executionLevels = new Dictionary<IFilter, int>();

        private FiltersExecutionPipeline()
        {
            
        }
        
        /// <summary>
        /// Build execution pipeline for filter. This method will order all filters in tree in right execution order.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [NotNull]
        public static FiltersExecutionPipeline Build(IFilter filter)
        {
            var pipeline = new FiltersExecutionPipeline();
            pipeline.Visit(filter);
            return pipeline;
        }
        
        private int Visit(IFilter filter)
        {
            switch (filter)
            {
                case CompoundFilter compoundFilter:
                    var maxRequiredLevel = 0; // Execution level of this node is max level of all operand nodes + 1
                    foreach (var operand in compoundFilter.Operands)
                    {
                        var operandRequiredLevel = Visit(operand);
                        if (operandRequiredLevel > maxRequiredLevel) 
                            maxRequiredLevel = operandRequiredLevel;
                    }
                    _executionLevels[filter] = maxRequiredLevel + 1;
                    break;
                case ComparisonFilter _:
                    // Base filter always has execution level 0 (it is always leaf)
                    _executionLevels[filter] = 0;
                    break;
                default:
                    if (!_executionLevels.ContainsKey(filter))
                        throw new ArgumentException($"Can't handle {filter} filter in {nameof(FiltersExecutionPipeline)}. " +
                                                    $"If new filters where added you need to extend {nameof(FiltersExecutionPipeline)}");
                    break;
            }

            return _executionLevels[filter];
        }
        
        /// <summary>
        /// Returns all filters grouped by execution groups and ordered by execution level.
        /// </summary>
        [NotNull]
        public ICollection<ExecutionGroup> GetExecutionGroups()
        {
            return _executionLevels
                .GroupBy(x => x.Value)
                .Select(x => new ExecutionGroup(x.Key, x.Select(y => y.Key).ToList()))
                .OrderBy(x => x.Level)
                .ToList();
        }
        
        public class ExecutionGroup
        {
            public ExecutionGroup(int level, ICollection<IFilter> filters)
            {
                Level = level;
                Filters = filters;
            }

            public int Level { get; }

            public ICollection<IFilter> Filters { get; }
        }
    }
}