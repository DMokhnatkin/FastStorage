using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FastStorage.Core;
using JetBrains.Annotations;

namespace FastStorage.Collection
{
    /// <summary>
    /// Stores fast collection data + manages data ids.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FastCollectionItems<T> : IEnumerable<FastCollectionItem<T>>
    {
        public IFastCollection BoundedFastCollection { get; }
        
        [NotNull]
        public ReadOnlyCollection<FastCollectionItem<T>> Data => new ReadOnlyCollection<FastCollectionItem<T>>(_data);
        
        [NotNull]
        private readonly List<FastCollectionItem<T>> _data = new List<FastCollectionItem<T>>();
        
        /// <summary>
        /// Add data to storage (ids will be auto generated)
        /// </summary>
        /// <param name="data"></param>
        public void Add([NotNull] IEnumerable<T> data)
        {
            _data.AddRange(data.Select((x, i) => new FastCollectionItem<T>(i, x)));
        }

        public FastCollectionItems(IFastCollection boundedFastCollection)
        {
            BoundedFastCollection = boundedFastCollection;
        }

        /// <inheritdoc />
        public IEnumerator<FastCollectionItem<T>> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}