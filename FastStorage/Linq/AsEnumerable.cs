using System;
using System.Collections.Generic;
using System.Linq;
using FastStorage.Core.Enumerable;
using JetBrains.Annotations;

namespace FastStorage.Linq
{
    public static partial class FastCollectionEnumerable
    {
        [NotNull]
        [PublicAPI]
        public static IEnumerable<T> AsEnumerable<T>(
            [NotNull] this IFastCollectionEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var baseEnumerable = source.GetEnumerator().AsIterator();
            return baseEnumerable.ItemsEnumerable.Select(x => x.Data);
        }
    }
}