using Scheduler.Entities.General;

namespace Scheduler.DataAccess.General;

public class AudienceRepository : GeneralRepository<Audience>
{
    protected override Func<GeneralData, Dictionary<Guid, Audience>> GetData => data => data.Audiences;
}