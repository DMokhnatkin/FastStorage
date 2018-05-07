using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace FastStorage.Core.Collections
{
    /// <summary>
    /// In this dictionary each key can have more than one related value.
    /// </summary>
    [PublicAPI]
    public class MultiValueDictionary<TKey, TValue> : IDictionary<TKey, IEnumerable<TValue>>
    {
        [NotNull]
        private readonly IDictionary<TKey, IEnumerable<TValue>> _dictionaryImplementation;

        private int _fullCount;

        public MultiValueDictionary()
        {
            _dictionaryImplementation = new Dictionary<TKey, IEnumerable<TValue>>();
            _fullCount = 0;
        }

        #region IEnumerator<KeyValuePair<TKey, IEnumerable<TValue>>>
        IEnumerator<KeyValuePair<TKey, IEnumerable<TValue>>> IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>>.GetEnumerator()
        {
            return _dictionaryImplementation.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_dictionaryImplementation).GetEnumerator();
        }
        #endregion

        #region ICollection<KeyValuePair<TKey, IEnumerable<TValue>>>

        void ICollection<KeyValuePair<TKey, IEnumerable<TValue>>>.Add(KeyValuePair<TKey, IEnumerable<TValue>> item)
        {
            InsertNewKeyValues(item.Key, item.Value);
        }
        
        public void Clear()
        {
            _dictionaryImplementation.Clear();
            _fullCount = 0;
        }

        bool ICollection<KeyValuePair<TKey, IEnumerable<TValue>>>.Contains(KeyValuePair<TKey, IEnumerable<TValue>> item)
        {
            return _dictionaryImplementation.Contains(item);
        }

        void ICollection<KeyValuePair<TKey, IEnumerable<TValue>>>.CopyTo(KeyValuePair<TKey, IEnumerable<TValue>>[] array, int arrayIndex)
        {
            _dictionaryImplementation.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, IEnumerable<TValue>>>.Remove(KeyValuePair<TKey, IEnumerable<TValue>> item)
        {
            var res = true;
            foreach (var val in item.Value)
            {
                res = res && Remove(item.Key, val);
            }
            return res;
        }

        int ICollection<KeyValuePair<TKey, IEnumerable<TValue>>>.Count => _dictionaryImplementation.Count;

        bool ICollection<KeyValuePair<TKey, IEnumerable<TValue>>>.IsReadOnly => _dictionaryImplementation.IsReadOnly;

        #endregion

        #region IDictionary<TKey, IEnumerable<TValue>>

        void IDictionary<TKey, IEnumerable<TValue>>.Add(TKey key, IEnumerable<TValue> value)
        {
            InsertNewKeyValues(key, value);
        }

        bool IDictionary<TKey, IEnumerable<TValue>>.TryGetValue(TKey key, out IEnumerable<TValue> value)
        {
            return _dictionaryImplementation.TryGetValue(key, out value);
        }

        ICollection<IEnumerable<TValue>> IDictionary<TKey, IEnumerable<TValue>>.Values => _dictionaryImplementation.Values;

        #endregion

        void InsertNewKeyValues(TKey key, IEnumerable<TValue> value)
        {
            var enumerable = value as TValue[] ?? (value ?? throw new ArgumentNullException(nameof(value))).ToArray();
            _dictionaryImplementation.Add(key, enumerable);
            _fullCount += enumerable.Length;
        }

        bool RemoveKey(TKey key)
        {
            if (!_dictionaryImplementation.ContainsKey(key))
                return false;
            var data = _dictionaryImplementation[key];
            _dictionaryImplementation.Remove(key);
            _fullCount -= ((List<TValue>) data).Count;
            return true;
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionaryImplementation.ContainsKey(key);
        }

        public void Add(TKey key, TValue val)
        {
            if (_dictionaryImplementation.ContainsKey(key))
                ((List<TValue>)_dictionaryImplementation[key]).Add(val);
            else
                _dictionaryImplementation[key] = new List<TValue> { val };
            _fullCount++;
        }

        public bool Remove(TKey key)
        {
            return RemoveKey(key);
        }

        public bool Remove(TKey key, TValue val)
        {
            var removed = ((List<TValue>) _dictionaryImplementation[key]).Remove(val);
            if (removed && ((List<TValue>) _dictionaryImplementation[key]).Count == 0)
                Remove(key);
            _fullCount--;
            return removed;
        }

        [NotNull]
        public IEnumerable<TValue> this[TKey key]
        {
            get => _dictionaryImplementation[key];
            set => _dictionaryImplementation[key] = value;
        }

        public ICollection<TKey> Keys => _dictionaryImplementation.Keys;

        public IEnumerable<TValue> Values => _dictionaryImplementation.SelectMany(x => x.Value);

        /// <summary>
        /// Full count of values in collection.
        /// </summary>
        public int FullCount => _fullCount;
    }
}
