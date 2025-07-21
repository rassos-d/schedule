using Scheduler.Entities.Base;

namespace Scheduler.Entities.Schedule;

public class Schedule : EntityWithName
{
    public List<SchedulePage> Pages { get; set; } = [];
}