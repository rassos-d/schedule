using System.Text.Json;
using Scheduler.DataAccess.Base;
using Scheduler.Entities.Base;
using static Scheduler.Constants.FilePaths;

namespace Scheduler.DataAccess.General;

public abstract class GeneralRepository<T> : BaseRepository where T : EntityWithName
{
    private readonly GeneralData _data;
    protected abstract Func<GeneralData, Dictionary<Guid, T>> GetData { get; }
    
    private readonly string _filePath;

    protected GeneralRepository() : base(string.Empty)
    {
        _filePath = Path.Combine(DirectoryPath, GeneralFilePath);
        _data = File.Exists(_filePath) 
            ? JsonSerializer.Deserialize<GeneralData>(File.ReadAllText(_filePath), JsonOptions) ?? new GeneralData()
            : new GeneralData();
    }

    protected override void SaveChanges(Guid? id = null) => 
        File.WriteAllText(_filePath, JsonSerializer.Serialize(_data, JsonOptions));
    
    public void SaveChanges() => SaveChanges(null);
    
    public virtual T? Get(Guid id) => GetData(_data).GetValueOrDefault(id);
    
    public virtual List<T> GetAll() => GetData(_data).Values.ToList();

    public virtual void Upsert(T entity)
    {
        // if (GetData(Data).Values.Count(x => x.Name == entity.Name) > 1)
        //     throw new EntityAlreadyExistExceptions("Такое имя уже занято.");
        GetData(_data)[entity.Id] = entity;
    }

    public virtual void Delete(Guid id) => GetData(_data).Remove(id);
}