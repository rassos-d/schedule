using Scheduler.Entities.General;

namespace Scheduler.DataAccess.General;

public partial class GeneralRepository
{
    public Audience? GetAudience(Guid id) => _data.Audiences.GetValueOrDefault(id);
    
    public List<Audience> GetAllAudiences() => _data.Audiences.Values.ToList();
    
    public void UpsertAudience(Audience audience) => _data.Audiences[audience.Id] = audience;

    public void DeleteAudience(Guid id) => _data.Audiences.Remove(id);
}