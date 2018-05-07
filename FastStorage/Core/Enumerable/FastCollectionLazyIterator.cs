using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FastStorage.Expressions.OperationTargets;
using JetBrains.Annotations;

namespace FastStorage.Core.Enumerable
{
    /// <summary>
    /// This iterator provides an opportunity to build ItemsEnumerable on demand (by providing builder Func)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class FastCollectionLazyIterator<T> : IFastCollectionEnumerable<T>, IFastCollectionEnumerator<T>, IFastCollectionIterator<T>
    {
        [NotNull]
        private readonly Func<IEnumerable<FastCollectionItem<T>>> _itemsBuilder;

        /// <inheritdoc />
        public SimpleOperationTarget OperationTarget { get; }

        /// <inheritdoc />
        public IFastCollection SourceCollection { get; }

        private IEnumerable<FastCollectionItem<T>> _itemsEnumerable;
        /// <inheritdoc />
        public IEnumerable<FastCollectionItem<T>> ItemsEnumerable => _itemsEnumerable ?? (_itemsEnumerable = _itemsBuilder());

        public FastCollectionLazyIterator(
            IFastCollection sourceCollection, 
            SimpleOperationTarget operationTarget, 
            Func<IEnumerable<FastCollectionItem<T>>> itemsBuilder)
        {
            _itemsBuilder = itemsBuilder;
            OperationTarget = operationTarget ?? throw new ArgumentNullException(nameof(operationTarget));
            SourceCollection = sourceCollection ?? throw new ArgumentNullException(nameof(sourceCollection));
        }

        /// <inheritdoc />
        IFastCollectionEnumerator<T> IFastCollectionEnumerable<T>.GetEnumerator()
        {
            return this;
        }
    }
}