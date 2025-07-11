using Scheduler.DataAccess.General;
using Scheduler.Entities.Base;

public class DictionaryRepository<T> where T : Entity
{
    private Dictionary<Guid, T> Data { get; }

    public DictionaryRepository(Dictionary<Guid, T> data)
    {
        Data = data;
    }

    public T? Get(Guid id) => Data.GetValueOrDefault(id);
    
    public List<T> GetAll() => Data.Values.ToList();
    
    public void Add(T entity) => Data.Add(entity.Id, entity);
    
    public void Upsert(T entity) => Data[entity.Id] = entity;

    public void Delete(Guid id) => Data.Remove(id);
}