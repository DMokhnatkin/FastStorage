using System;
using System.Collections.Generic;
using System.Linq;
using FastStorage.Core.Collections;
using FastStorage.Core.Indices;
using JetBrains.Annotations;

namespace FastStorage.Indices
{
    public class HashTableIndexFactory : IIndexFactory
    {
        /// <inheritdoc />
        public IIndex<TKey, int> CreateIndex<TKey>()
        {
            return new HashTableIndex<TKey>();
        }
    }
    
    public class HashTableIndex<TKey> : IIndex<TKey, int>
    {
        [NotNull] public MultiValueDictionary<TKey, int> DataStruct => _data;
        
        [NotNull]
        private readonly MultiValueDictionary<TKey, int> _data = new MultiValueDictionary<TKey, int>();

        /// <inheritdoc />
        public Type KeyType => typeof(TKey);

        /// <inheritdoc />
        public Type ValueType => typeof(int);
        
        /// <inheritdoc />
        public void FillIndex(IEnumerable<IndexItem<TKey, int>> data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            foreach (var item in data)
            {
                _data.Add(item.Key, item.Value);
            }
        }
        /// <inheritdoc />
        public void FillIndex(IEnumerable<IIndexItem> data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            FillIndex(data.Select(x => x.WrapIndexItem<TKey, int>()));
        }

        /// <inheritdoc />
        public int Count { get; }
        
        public void Add(TKey key, int value)
        {
            _data.Add(key, value);
        }

        public void Remove(TKey key, int value)
        {
            _data.Remove(key, value);
        }

        [NotNull]
        public IEnumerable<int> Get(TKey key)
        {
            if (!_data.ContainsKey(key))
                return Enumerable.Empty<int>();
            return _data[key];
        }

        /// <summary>
        /// Does index contains any value with specified key
        /// </summary>
        public bool Contains(TKey key)
        {
            return Get(key).Any();
        }

        /// <summary>
        /// Does index contains any value with specified key
        /// </summary>
        public bool Contains(TKey key, int val)
        {
            return Get(key).Any(x => x.Equals(val));
        }
    }
}