using System;
using FastStorage.Execution.BuiltInExecutors;
using JetBrains.Annotations;

namespace FastStorage.Execution.ExecutorsCore
{
    /// <summary>
    /// This class maintains available operation providers collection.
    /// </summary>
    internal class ExecutorsManager
    {
        [NotNull] public AvailableExecutorsCollection AvailableExecutorsCollection { get; } = new AvailableExecutorsCollection();
        
        [NotNull]
        private static readonly Lazy<ExecutorsManager> _instance = new Lazy<ExecutorsManager>(() => new ExecutorsManager(), true);

        [NotNull]
        public static ExecutorsManager Instance => _instance.Value;
        
        public void RegisterModule([CanBeNull] IOperationExecutorModule operationExecutorModule)
        {
            operationExecutorModule?.Register(AvailableExecutorsCollection);
        }

        static ExecutorsManager()
        {
            // TODO: extract
            Instance.RegisterModule(new RBTreeExecutorsModule());
            Instance.RegisterModule(new HashTableExecutorModule());
        }
    }
}