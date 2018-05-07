using FastStorage.Core;
using JetBrains.Annotations;

namespace FastStorage.Execution.ExecutorsCore
{
    internal interface IOperationExecutor
    {
        /// <summary>
        /// Prepares execution (calculate expected complexity, creates execution action callback) using this provider.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        PreparedExecution PrepareExecution(IOperation operation);
    }
}