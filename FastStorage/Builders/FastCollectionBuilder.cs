using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FastStorage.Collection;
using FastStorage.Core.Indices;
using FastStorage.Expressions.OperationTargets;
using JetBrains.Annotations;

namespace FastStorage.Builders
{
    public static class FastCollectionBuilderExtensions
    {
        /// <summary>
        /// Create fast collection builder (for specifed data).
        /// </summary>
        /// <param name="data"></param>
        /// <param name="target"></param>
        /// <param name="indexFactories"></param>
        /// <typeparam name="TRaw"></typeparam>
        /// <typeparam name="TIndexKey">Type of index key</typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NotNull]
        public static FastCollectionBuilderFlow<TRaw> AddIndex<TRaw, TIndexKey>(
            [NotNull]
            this IEnumerable<TRaw> data, 
            [NotNull]
            Expression<Func<TRaw, TIndexKey>> target,
            params IIndexFactory[] indexFactories)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (indexFactories == null) throw new ArgumentNullException(nameof(indexFactories));

            var fastCollection = new FastCollection<TRaw>();
            fastCollection.Items.Add(data);
            var nextFlow = new FastCollectionBuilderFlow<TRaw>(fastCollection);
            nextFlow.AddIndex(target, indexFactories);
            return nextFlow;
        }
        
        /// <summary>
        /// Build fast collection without any indices.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public static FastCollection<TRaw> ToFastCollection<TRaw>([NotNull] this IEnumerable<TRaw> data)
        {
            var res = new FastCollection<TRaw>();
            res.Items.Add(data);
            return res;
        }
    }

    public class FastCollectionBuilderFlow<TRaw>
    {
        [NotNull] private readonly FastCollection<TRaw> _fastCollection;

        internal FastCollectionBuilderFlow([NotNull] FastCollection<TRaw> fastCollection)
        {
            _fastCollection = fastCollection;
        }
        
        private static void FillIndex<TIndexKey>(IIndex<TIndexKey, int> index, [NotNull] FastCollection<TRaw> fastCollection, [NotNull] Func<TRaw, TIndexKey> selector)
        {
            var indexData = new List<IndexItem<TIndexKey, int>>();
            foreach (var item in fastCollection.Items)
            {
                indexData.Add(new IndexItem<TIndexKey, int>(selector(item.Data), item.Id));
            }
            index.FillIndex(indexData);
        }
        
        [NotNull]
        public FastCollectionBuilderFlow<TRaw> AddIndex<TIndexKey>(
            [NotNull] Expression<Func<TRaw, TIndexKey>> target,
            [NotNull] params IIndexFactory[] indexFactories)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (indexFactories == null) throw new ArgumentNullException(nameof(indexFactories));

            foreach (var indexFactory in indexFactories)
            {
                var index = indexFactory.CreateIndex<TIndexKey>();
                var operationTarget = OperationTargetBuilder.BuildForSelector(_fastCollection, target);
                FillIndex(index, _fastCollection, target.Compile());
                _fastCollection.IndicesCoverageInternal.AddIndex(operationTarget, index);
            }
            return this;
        }
        
        /// <summary>
        /// Build fast collection and fill indices with data.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public FastCollection<TRaw> Build()
        {
            return _fastCollection;
        }
    }
}
