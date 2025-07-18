using Scheduler.Entities.Base;

namespace Scheduler.DataAccess.Base;

public class DictionaryRepository<T> where T : Entity
{
    private Dictionary<Guid, T> Data { get; }

    public DictionaryRepository(Dictionary<Guid, T> data)
    {
        Data = data;
    }

    public T? Get(Guid id) => Data.GetValueOrDefault(id);
    
    public List<T> GetAll() => Data.Values.ToList();
    
    public void Upsert(T entity) => Data[entity.Id] = entity;

    public void Delete(Guid id) => Data.Remove(id);
}