namespace FastStorage.Core.Indices
{
    public interface IIndexFactory
    {
        IIndex<TKey, int> CreateIndex<TKey>();
    }
}