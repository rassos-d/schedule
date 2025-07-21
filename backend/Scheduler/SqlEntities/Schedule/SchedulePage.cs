using Scheduler.Dto.Constants;
using Scheduler.SqlEntities.Base;
using Scheduler.SqlEntities.General;

namespace Scheduler.SqlEntities.Schedule;

public class SchedulePage : Entity
{
    public required Guid ScheduleId { get; init; }
    public StudyYear StudyYear { get; set; }
    public Semester Semester { get; set; }
    
    public List<DateOnly> Dates { get; set; } = [];
    
    public List<Squad> Squads { get; set; } = [];
    public List<Event> Events { get; set; } = [];
}