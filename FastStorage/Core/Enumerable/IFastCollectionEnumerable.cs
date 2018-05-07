using System.Collections.Generic;
using JetBrains.Annotations;

namespace FastStorage.Core.Enumerable
{
    public interface IFastCollectionEnumerable<T>
    {
        [NotNull]
        IFastCollectionEnumerator<T> GetEnumerator();
    }
    
    public interface IFastCollectionEnumerator<T>
    {
        
    }
}