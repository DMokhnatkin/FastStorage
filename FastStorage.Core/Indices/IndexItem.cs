using FastStorage.Core.Helpers;
using JetBrains.Annotations;

namespace FastStorage.Core.Indices
{
    /// <summary>
    /// Index stores some key/value pairs and optimize some operation over key. (f.e. range search over key)
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [PublicAPI]
    public class IndexItem <TKey, TValue> : IIndexItem<TKey, TValue>, IIndexItem
    {
        public TKey Key { get; }

        public TValue Value { get; }

        object IIndexItem.Key => Key;

        object IIndexItem.Value => Value;

        public IndexItem(TKey key, TValue val)
        {
            Key = key;
            Value = val;
        }

        private bool EqualsInternal([NotNull] IndexItem<TKey, TValue> other)
        {
            return Key.Equals(other.Key) &&
                   Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is IndexItem<TKey, TValue> tempOther))
                return false;
            return EqualsInternal(tempOther);
        }

        public override int GetHashCode()
        {
            return CommonHelpers.CalcHashCode(Key, Value);
        }
    }
}
