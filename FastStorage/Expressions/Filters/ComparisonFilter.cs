using System;
using FastStorage.Expressions.OperationTargets;
using JetBrains.Annotations;

namespace FastStorage.Expressions.Filters
{
    /// <summary>
    /// Comparison filter (f.e. x > 3)
    /// </summary>
    internal class ComparisonFilter : IOneOperationTargetFilter
    {
        /// <inheritdoc />
        [NotNull]
        public SimpleOperationTarget OperationTarget { get; }
        
        public ComparisonFilterType ComparisonFilterType { get; }

        [NotNull]
        public object Value { get; }
        
        public ComparisonFilter([NotNull] SimpleOperationTarget operationTarget, ComparisonFilterType comparisonFilterType, [NotNull] object value)
        {
            OperationTarget = operationTarget ?? throw new ArgumentNullException(nameof(operationTarget));   
            ComparisonFilterType = comparisonFilterType;
            Value = value?? throw new ArgumentNullException(nameof(value));
        }
    }
    
    public enum ComparisonFilterType
    {
        None = 0,
        Less = 1,
        LessOrEqual = 2,
        Greater = 3,
        GreaterOrEqual = 4,
        Equal = 5,
        NotEqual = 6
    }

    internal static class FilterTypeExtensions
    {
        public static ComparisonFilterType Mirror(this ComparisonFilterType source)
        {
            switch (source)
            {
                case ComparisonFilterType.Less:
                    return ComparisonFilterType.Greater;
                case ComparisonFilterType.LessOrEqual:
                    return ComparisonFilterType.GreaterOrEqual;
                case ComparisonFilterType.Greater:
                    return ComparisonFilterType.Less;
                case ComparisonFilterType.GreaterOrEqual:
                    return ComparisonFilterType.LessOrEqual;
                default:
                    return ComparisonFilterType.None;
            }
        }
    }
}
