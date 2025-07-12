using Scheduler.Entities.Base;

namespace Scheduler.Entities.Schedule;

public class Schedule : Entity
{
    public required string Name { get; set; }
    
    public List<StudyYearPage> Pages { get; set; } = [];
}