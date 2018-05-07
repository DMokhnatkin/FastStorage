using FastStorage.Core;
using FastStorage.Expressions.OperationTargets;

namespace FastStorage.Expressions.Filters
{
    /// <summary>
    /// Filter - inner presentaion of some "where" predicate
    /// </summary>
    internal interface IFilter : IOperation
    {
    }

    /// <summary>
    /// Filters with one operation target
    /// </summary>
    internal interface IOneOperationTargetFilter : IFilter
    {
        SimpleOperationTarget OperationTarget { get; }
    }
}