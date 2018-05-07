using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FastStorage.Core;
using FastStorage.Core.Enumerable;
using FastStorage.Execution;
using FastStorage.Expressions.Filters;
using JetBrains.Annotations;

namespace FastStorage.Linq
{
    public static partial class FastCollectionEnumerable
    {
        [NotNull]
        [PublicAPI]
        public static IFastCollectionEnumerable<TSource> Where<TSource>(
            [NotNull] this IFastCollectionEnumerable<TSource> source,
            [NotNull] Expression<Func<TSource, bool>> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var sourceIterator = source.ExtractIterator();

            var filter = new FiltersParser().ParsePredicate(predicate, sourceIterator.OperationTarget);
            var filtersExecutor = new FiltersExecutor();
            var preparedFilterExecution = filtersExecutor.PrepareExecution(filter, sourceIterator.SourceCollection.IndicesCoverage);
            if (preparedFilterExecution.CanBeExecuted)
            {
                // We can use indices for execution
                return new FastCollectionLazyIterator<TSource>(
                    sourceIterator.SourceCollection,
                    sourceIterator.OperationTarget,
                    () =>
                    {
                        // This lambda will be called on items demand
                        var cuttedIds = new HashSet<int>(preparedFilterExecution.ExecuteAction());
                        return sourceIterator.ItemsEnumerable.Where(x => cuttedIds.Contains(x.Id));
                    });
            }
            else
            {
                // In this case we can't use indices for execution and we have to check each item
                var compiledPredicate = predicate.Compile();
                var newItemsEnumerable = sourceIterator.ItemsEnumerable.Where(x => compiledPredicate(x.Data));
            
                var fastCollectionWhereEnumerable = new FastCollectionIterator<TSource>(sourceIterator.SourceCollection, sourceIterator.OperationTarget, newItemsEnumerable);
                return fastCollectionWhereEnumerable;
            }
        }
    }
}