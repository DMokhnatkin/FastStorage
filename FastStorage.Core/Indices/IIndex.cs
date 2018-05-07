using System;
using System.Collections.Generic;

namespace FastStorage.Core.Indices
{
    public interface IIndex<TKey, TValue> : IIndex
    {
        /// <summary>
        /// Generic version of <see cref="IIndex.FillIndex"/>
        /// </summary>
        void FillIndex(IEnumerable<IndexItem<TKey, TValue>> data);
    }

    public interface IIndex
    {
        Type KeyType { get; }

        Type ValueType { get; }

        /// <summary>
        /// Fill index with values
        /// </summary>
        void FillIndex(IEnumerable<IIndexItem> data);

        int Count { get; }
    }
}
