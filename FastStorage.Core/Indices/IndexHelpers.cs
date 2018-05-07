using System;
using JetBrains.Annotations;

namespace FastStorage.Core.Indices
{
    public static class IndexHelpers
    {
        private static void CastKeyAndValue<TKey, TValue>([NotNull] IIndexItem item, out TKey key, out TValue value)
        {
            if (!(item.Key is TKey tmpKey) || !(item.Value is TValue tmpVal))
                throw new ArgumentException($"{item} has invalid key/value type. Expected key : {typeof(TKey)} value : {typeof(TValue)}");
            key = tmpKey;
            value = tmpVal;
        }

        public static IndexItem<TKey, TValue> WrapIndexItem<TKey, TValue>(this IIndexItem self)
        {
            if (self is IndexItem<TKey, TValue> tmpIndex)
                return tmpIndex;
            CastKeyAndValue(self, out TKey tmpKey, out TValue tmpValue);
            return new IndexItem<TKey, TValue>(tmpKey, tmpValue);
        }
    }
}
