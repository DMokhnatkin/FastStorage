using System;
using System.Linq;
using System.Linq.Expressions;
using FastStorage.Core;
using FastStorage.Core.Enumerable;
using FastStorage.Expressions.OperationTargets;
using JetBrains.Annotations;

namespace FastStorage.Linq
{
    public static partial class FastCollectionEnumerable
    {
        [NotNull]
        [PublicAPI]
        public static IFastCollectionEnumerable<TResult> Select<TSource, TResult>(
            [NotNull] this IFastCollectionEnumerable<TSource> source,
            [NotNull] Expression<Func<TSource, TResult>> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var sourceIterator = source.ExtractIterator();
            
            // In linq pipeline:
            // 1) Select clause changes operation target
            var newOperationTarget = sourceIterator.OperationTarget.ExtendWithSelector(selector);
            
            // 2) Select clause changes data
            var compiledSelector = selector.Compile();
            var newItemsEnumerable = sourceIterator.ItemsEnumerable.Select(x => new FastCollectionItem<TResult>(x.Id, compiledSelector(x.Data)));
            
            var fastCollectionSelectEnumerable = new FastCollectionIterator<TResult>(sourceIterator.SourceCollection, newOperationTarget, newItemsEnumerable);
            return fastCollectionSelectEnumerable;
        }
    }
}