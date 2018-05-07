using System;
using FastStorage.Execution.ExecutorsCore.Complexity;
using JetBrains.Annotations;

namespace FastStorage.Execution.ExecutorsCore
{
    /// <summary>
    /// This class stored prepared operation execution (with its time complexity).
    /// So calling code can check if operation can be executed, and expected time complexity without real executing.
    /// </summary>
    internal class PreparedExecution
    {
        /// <summary>
        /// True if operation can be executed, otherwise false
        /// </summary>
        public bool CanBeExecuted { get; }
        
        /// <summary>
        /// Expected execution time complexity
        /// </summary>
        public IComplexity TimeComplexity { get; }

        /// <summary>
        /// It is callback for real execution.
        /// </summary>
        public Func<int[]> ExecuteAction { get; }
        
        public PreparedExecution(bool canBeExecuted, IComplexity timeComplexity, Func<int[]> executeAction)
        {
            TimeComplexity = timeComplexity;
            ExecuteAction = executeAction;
            CanBeExecuted = canBeExecuted;
        }
        
        [NotNull]
        public static PreparedExecution Unsuccess => new PreparedExecution(false, null, null);
    }
}