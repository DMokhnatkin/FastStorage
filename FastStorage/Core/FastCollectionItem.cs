namespace FastStorage.Core
{
    /// <summary>
    /// This class wraps data stored in fast collection.
    /// Main goal is to keep id near data (when we will apply linq methods we will need ids)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FastCollectionItem<T>
    {
        public FastCollectionItem(int id, T data)
        {
            Data = data;
            Id = id;
        }

        public int Id { get; }

        public T Data { get; }
    }
}