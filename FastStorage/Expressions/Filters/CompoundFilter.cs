using System.Collections.Generic;
using JetBrains.Annotations;

namespace FastStorage.Expressions.Filters
{
    internal class CompoundFilter : IFilter
    {
        [NotNull]
        public IEnumerable<IFilter> Operands { get; }

        public CompoundFilterType OperationType { get; }
        
        public enum CompoundFilterType
        {
            None = 0,
            And = 1,
            Or = 2
        }

        public CompoundFilter(CompoundFilterType operationType, [NotNull] IEnumerable<IFilter> operands)
        {
            Operands = operands;
            OperationType = operationType;
        }
        
        internal CompoundFilter(CompoundFilterType operationType, params IFilter[] operands)
        {
            Operands = operands;
            OperationType = operationType;
        }
    }
}