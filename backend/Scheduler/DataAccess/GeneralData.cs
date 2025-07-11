using Scheduler.Entities.General;

namespace Scheduler.DataAccess;

public class GeneralData
{
    public List<Audience> Audiences { get; set; } = [];
    public List<Squad> Squads { get; set; } = [];
    public List<Teacher> Teachers { get; set; } = [];
}