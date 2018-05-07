using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FastStorage.Core.Indices;
using JetBrains.Annotations;

namespace FastStorage.Execution.ExecutorsCore
{
    /// <summary>
    /// Contains logic about storing and resolving executors.
    /// </summary>
    internal class AvailableExecutorsCollection
    {
        [NotNull]
        private readonly Dictionary<Type, Type[]> _availableExecutorTypes = new Dictionary<Type, Type[]>();

        public void Register(Type operationExecutorType)
        {
            if (!typeof(IOperationExecutor).IsAssignableFrom(operationExecutorType))
                throw new ArgumentException($"Can't register {operationExecutorType} as operation executor. Each executor must implement {nameof(IOperationExecutor)}");
            
            var ctors = operationExecutorType.GetConstructors();
            if (ctors?.Length != 1)
                throw new ArgumentException($"Operation executor can have one and only one constructor, but {operationExecutorType} has {ctors?.Length}");

            var ctorParameters = ctors[0].GetParameters();
            var nonIndexParameter = ctorParameters.FirstOrDefault(x => !typeof(IIndex).IsAssignableFrom(x.ParameterType)); 
            if (nonIndexParameter != null)
                throw new ArgumentException($"All parameters of operation executor constrcutor must be inherited from {nameof(IIndex)}, but {nonIndexParameter} wasn't.");
            
            if (_availableExecutorTypes.ContainsKey(operationExecutorType))
                throw new ArgumentException($"{operationExecutorType} has already been registered.");
            _availableExecutorTypes.Add(operationExecutorType, ctorParameters.Select(x => x.ParameterType).ToArray());
        }
        
        /// <summary>
        /// Returns all registered operation executors which index requirements are satisfied by specified indices collection.
        /// </summary>
        /// <param name="indices"></param>
        /// <returns></returns>
        public ICollection<IOperationExecutor> GetAvailableExecutors([NotNull] Type indexKeyType, ICollection<IIndex> indices)
        {
            var res = new List<IOperationExecutor>();
            foreach (var availableExecutorType in _availableExecutorTypes)
            {
                var requiredIndexTypes = availableExecutorType.Value; // index types required by this executor
                // TODO: more smart comparation + fast search or cache
                if (requiredIndexTypes.All(x => indices.Any(y => x.Name == y.GetType().Name)))
                {
                    var foundRequiredIndices = requiredIndexTypes
                        .Select(x => indices.First(y => x.Name == y.GetType().Name)).OfType<object>().ToArray();
                    if (availableExecutorType.Key.ContainsGenericParameters)
                    {
                        res.Add((IOperationExecutor)Activator.CreateInstance(availableExecutorType.Key.MakeGenericType(indexKeyType), foundRequiredIndices)); 
                    }
                    else
                    {
                        res.Add((IOperationExecutor)Activator.CreateInstance(availableExecutorType.Key, foundRequiredIndices)); 
                    }
                }
            }

            return res;
        }
    }
}