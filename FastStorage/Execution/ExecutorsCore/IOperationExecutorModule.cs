using JetBrains.Annotations;

namespace FastStorage.Execution.ExecutorsCore
{
    /// <summary>
    /// OperationExecutorModule - is object which can register operation executors.
    /// </summary>
    internal interface IOperationExecutorModule
    {
        void Register([NotNull] AvailableExecutorsCollection availableExecutorsCollection);
    }
}