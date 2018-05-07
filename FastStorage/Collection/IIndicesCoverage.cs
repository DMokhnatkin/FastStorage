using System.Collections.Generic;
using FastStorage.Core.Indices;
using FastStorage.Expressions.OperationTargets;

namespace FastStorage.Collection
{
    public interface IIndicesCoverage
    {
        ICollection<IIndex> GetIndices(IOperationTarget operationTarget);
    }
}