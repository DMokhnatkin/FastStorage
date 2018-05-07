namespace FastStorage.Core.Indices
{
    public interface IIndexItem<TKey, TValue>
    {
        TKey Key { get; }

        TValue Value { get; }
    }
    
    public interface IIndexItem
    {
        object Key { get; }

        object Value { get; }
    }
}