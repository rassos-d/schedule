using Scheduler.Entities.General;

namespace Scheduler.DataAccess;

public class GeneralData
{
    public Dictionary<Guid, Audience> Audiences { get; set; } = [];
    public Dictionary<Guid, Squad> Squads { get; set; } = [];
    public Dictionary<Guid, Teacher> Teachers { get; set; } = [];
}