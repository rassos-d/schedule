using Scheduler.Entities.General;

namespace Scheduler.DataAccess.General;

public class SquadRepository : GeneralRepository<Squad>
{
    protected override Func<GeneralData, Dictionary<Guid, Squad>> GetData =>  data => data.Squads;

    public List<Squad> GetSquadsByIds(IEnumerable<Guid> squadIds)
    {
        return Data
            .Squads.Where(x => squadIds.Contains(x.Key))
            .Select(x => x.Value)
            .ToList();
    }
}