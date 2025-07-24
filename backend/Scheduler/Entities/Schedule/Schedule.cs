using Scheduler.Entities.Base;

namespace Scheduler.Entities.Schedule;

public class Schedule : EntityWithName
{
    public int Semester { get; set; }
    public List<SchedulePage> Pages { get; set; } = [];
}