using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FastStorage.Core;
using FastStorage.Core.Enumerable;
using FastStorage.Expressions.OperationTargets;
using JetBrains.Annotations;

namespace FastStorage.Collection
{
    public class FastCollection<TData> : IFastCollection, IFastCollection<TData>, IFastCollectionEnumerable<TData>
    {
        [NotNull]
        internal IndicesCoverage IndicesCoverageInternal { get; }
        
        public IIndicesCoverage IndicesCoverage => IndicesCoverageInternal;
        
        [NotNull]
        internal FastCollectionItems<TData> Items { get; }
        
        internal FastCollection()
        {
            IndicesCoverageInternal = new IndicesCoverage(this);
            Items = new FastCollectionItems<TData>(this);
        }

        /// <inheritdoc />
        public IFastCollectionEnumerator<TData> GetEnumerator()
        {
            return new FastCollectionIterator<TData>(this, OperationTargetBuilder.BuildForRoot(this), Items);
        }

        /// <inheritdoc />
        public Type DataType => typeof(TData);
    }
}