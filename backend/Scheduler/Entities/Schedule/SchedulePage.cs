using Scheduler.Dto.Constants;
using Scheduler.Entities.Constants;

namespace Scheduler.Entities.Schedule;

public class SchedulePage
{
    public required Guid ScheduleId { get; init; }
    public StudyYear StudyYear { get; set; }
    public Semester Semester { get; set; }
    
    public List<DateOnly> Dates { get; set; } = [];
    
    public List<Guid> Squads { get; set; } = [];
    public List<Event> Events { get; set; } = [];
}