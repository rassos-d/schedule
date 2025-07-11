using Scheduler.Entities.General;

namespace Scheduler.DataAccess.General;

public partial class GeneralRepository
{
    public Audience? GetAudience(Guid id) => _data.Audiences.FirstOrDefault(a => a.Id == id);
    
    public List<Audience> GetAllAudiences() => _data.Audiences;
    
    public void AddAudience(Audience audience) => _data.Audiences.Add(audience);
    
    public bool UpdateAudience(Audience audience)
    {
        var index = _data.Audiences.FindIndex(a => a.Id == audience.Id);
        if (index == -1) return false;
        _data.Audiences[index] = audience;
        return true;
    }
    
    public bool DeleteAudience(Guid id)
    {
        var audience = GetAudience(id);
        if (audience == null) return false;
        return _data.Audiences.Remove(audience);
    }
}