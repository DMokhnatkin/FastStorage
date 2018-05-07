using System.Collections.Generic;
using FastStorage.Expressions.OperationTargets;
using JetBrains.Annotations;

namespace FastStorage.Core.Enumerable
{
    internal interface IFastCollectionIterator<T>
    {
        [NotNull]
        SimpleOperationTarget OperationTarget { get; }
        
        [NotNull]
        IFastCollection SourceCollection { get; }
        
        [NotNull]
        IEnumerable<FastCollectionItem<T>> ItemsEnumerable { get; }
    }
}