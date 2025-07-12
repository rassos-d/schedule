using Scheduler.Entities.Base;
using Scheduler.Entities.Constants;

namespace Scheduler.Entities.Schedule;

public class StudyYearPage : Entity
{
    public StudyYear StudyYear { get; set; }
    
    public List<DateTime> Dates { get; set; } = [];
    public List<Event> Events { get; set; } = [];
}