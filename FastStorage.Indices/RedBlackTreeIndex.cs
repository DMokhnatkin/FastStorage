using System;
using System.Collections.Generic;
using FastStorage.Algorithms;
using FastStorage.Core.Indices;
using JetBrains.Annotations;

namespace FastStorage.Indices
{
    public class RedBlackTreeIndexFactory : IIndexFactory
    {
        /// <inheritdoc />
        public IIndex<TKey, int> CreateIndex<TKey>()
        {
            return new RedBlackTreeIndex<TKey>();
        }
    }
    
    public class RedBlackTreeIndex<TKey> : IIndex<TKey, int>
    {
        [NotNull]
        public RedBlackTree<TKey, int> DataStruct { get; }
        
        /// <inheritdoc />
        public Type KeyType => typeof(TKey);

        /// <inheritdoc />
        public Type ValueType => typeof(int);

        public RedBlackTreeIndex()
        {
            DataStruct = new RedBlackTree<TKey, int>();
        }
        
        /// <inheritdoc/>
        public void FillIndex([CanBeNull] IEnumerable<IndexItem<TKey, int>> data)
        {
            foreach (var indexItem in data ?? new IndexItem<TKey, int>[0])
            {
                DataStruct.Insert(indexItem.Key, indexItem.Value);
            }
        }

        /// <inheritdoc/>
        public void FillIndex(IEnumerable<IIndexItem> data)
        {
            foreach (var indexItem in data ?? new IndexItem<TKey, int>[0])
            {
                if (!(indexItem.Key is TKey) || !(indexItem.Key is int))
                    throw new ArgumentException("Inalid type of index item key/value");
                DataStruct.Insert((TKey)indexItem.Key, (int)indexItem.Value);
            }
        }

        /// <inheritdoc />
        public int Count => DataStruct.ItemsCt;
    }
}