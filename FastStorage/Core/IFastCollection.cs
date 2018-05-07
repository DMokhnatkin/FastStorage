using System;
using FastStorage.Collection;
using FastStorage.Core.Enumerable;
using JetBrains.Annotations;

namespace FastStorage.Core
{
    public interface IFastCollection<TData> : IFastCollection, IFastCollectionEnumerable<TData>
    {
        
    }
    
    public interface IFastCollection
    {
        /// <summary>
        /// Type of data stored in fast colelction
        /// </summary>
        [NotNull]
        Type DataType { get; }
        
        /// <summary>
        /// <see cref="IIndicesCoverage"/> stores all indices created for fast collection
        /// </summary>
        [NotNull]
        IIndicesCoverage IndicesCoverage { get; }
    }
}
