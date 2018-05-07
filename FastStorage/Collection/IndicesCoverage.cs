using System;
using System.Collections.Generic;
using System.Linq;
using FastStorage.Core;
using FastStorage.Core.Collections;
using FastStorage.Core.Indices;
using FastStorage.Expressions.OperationTargets;
using JetBrains.Annotations;

namespace FastStorage.Collection
{
    /// <summary>
    /// Collection which stores several indices over several indexTargets.
    /// </summary>
    internal class IndicesCoverage : IIndicesCoverage
    {
        public IFastCollection BoundedCollection { get; }
        
        [NotNull]
        private readonly MultiValueDictionary<IOperationTarget, IIndex> _indicesCoverage;

        internal IndicesCoverage(IFastCollection boundedCollection)
        {
            BoundedCollection = boundedCollection ?? throw new ArgumentNullException(nameof(boundedCollection));
            _indicesCoverage = new MultiValueDictionary<IOperationTarget, IIndex>();
        }
        
        internal void Clear()
        {
            _indicesCoverage.Clear();
        }

        internal void AddIndex(SimpleOperationTarget operationTarget, IIndex index)
        {
            _indicesCoverage.Add(operationTarget, index);
        }

        /// <inheritdoc />
        public ICollection<IIndex> GetIndices(IOperationTarget operationTarget)
        {
            if (!_indicesCoverage.ContainsKey(operationTarget))
                return new IIndex[0];
            return _indicesCoverage[operationTarget].ToArray();
        }
    }
}
