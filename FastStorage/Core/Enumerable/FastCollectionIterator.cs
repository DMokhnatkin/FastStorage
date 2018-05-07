using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FastStorage.Expressions.OperationTargets;
using JetBrains.Annotations;

namespace FastStorage.Core.Enumerable
{
    /// <summary>
    /// Simple iterator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class FastCollectionIterator<T> : IFastCollectionEnumerable<T>, IFastCollectionEnumerator<T>, IFastCollectionIterator<T>
    {
        internal FastCollectionIterator(
            [NotNull] IFastCollection sourceCollection, 
            [NotNull] SimpleOperationTarget operationTarget, 
            [NotNull] IEnumerable<FastCollectionItem<T>> itemsEnumerable)
        {
            SourceCollection = sourceCollection;
            OperationTarget = operationTarget;
            ItemsEnumerable = itemsEnumerable;
        }

        [NotNull]
        public SimpleOperationTarget OperationTarget { get; }
        
        [NotNull]
        public IFastCollection SourceCollection { get; }
        
        [NotNull]
        public IEnumerable<FastCollectionItem<T>> ItemsEnumerable { get; }

        /// <inheritdoc />
        public IFastCollectionEnumerator<T> GetEnumerator()
        {
            return this;
        }
    }

    internal static class FastCollectionIteratorExtensions
    {
        [NotNull]
        public static IFastCollectionIterator<T> AsIterator<T>([NotNull] this IFastCollectionEnumerator<T> source)
        {
             if (source is IFastCollectionIterator<T> res)
                return res;
            throw new InvalidOperationException($"Can't cast {nameof(IFastCollectionEnumerator<T>)} to {nameof(FastCollectionIterator<T>)}. " +
                                                $"All {nameof(IFastCollectionEnumerator<T>)} must be inherited from {nameof(FastCollectionIterator<T>)}.");
        }
        
        [NotNull]
        public static IFastCollectionIterator<T> ExtractIterator<T>([NotNull] this IFastCollectionEnumerable<T> source)
        {
            return source.GetEnumerator().AsIterator();
        }
    }
}