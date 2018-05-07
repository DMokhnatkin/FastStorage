using FastStorage.Execution.ExecutorsCore;

namespace FastStorage.Execution.BuiltInExecutors
{
    internal class RBTreeExecutorsModule : IOperationExecutorModule
    {
        /// <inheritdoc />
        public void Register(AvailableExecutorsCollection availableExecutorsCollection)
        {
            availableExecutorsCollection.Register(typeof(RBTreeExecutor<>));
        }
    }
}