using System;
using System.Linq;
using FastStorage.Core;
using FastStorage.Core.Enumerable;
using JetBrains.Annotations;

namespace FastStorage.Linq
{
    public static partial class FastCollectionEnumerable
    {
        [NotNull]
        [PublicAPI]
        public static TSource[] ToArray<TSource>([NotNull] this IFastCollectionEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.AsEnumerable().ToArray(); 
        }
    }
}