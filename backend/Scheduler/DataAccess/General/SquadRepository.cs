using Scheduler.Entities.General;

namespace Scheduler.DataAccess.General;

public class SquadRepository : GeneralRepository<Squad>
{
    protected override Func<GeneralData, Dictionary<Guid, Squad>> GetData =>  data => data.Squads;
}