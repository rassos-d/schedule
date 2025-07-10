using Scheduler.Models.General;

namespace Scheduler.Data;

public class GeneralData
{
    public List<Audience> Audiences { get; set; } = [];
    public List<Squad> Squads { get; set; } = [];
    public List<Teacher> Teachers { get; set; } = [];
}