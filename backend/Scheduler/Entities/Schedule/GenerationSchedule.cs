using Scheduler.Entities.General;
using Scheduler.Models;

namespace Scheduler.Entities.Schedule;

public class GenerationSchedule
{
    public List<Teacher> Teachers { get; set; }

    public List<DateOnly> Dates { get; set; }

    // вернуть отсортированные
    public List<Squad> Squads { get; set; }
    
    public List<DirectionInfo>  Directions { get; set; }
}