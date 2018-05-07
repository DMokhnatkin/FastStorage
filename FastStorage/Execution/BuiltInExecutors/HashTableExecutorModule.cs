using FastStorage.Execution.ExecutorsCore;

namespace FastStorage.Execution.BuiltInExecutors
{
    internal class HashTableExecutorModule : IOperationExecutorModule
    {
        /// <inheritdoc />
        public void Register(AvailableExecutorsCollection availableExecutorsCollection)
        {
            availableExecutorsCollection.Register(typeof(HashTableExecutor<>));
        }
    }
}