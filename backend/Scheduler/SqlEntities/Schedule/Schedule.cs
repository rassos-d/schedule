using Scheduler.SqlEntities.Base;

namespace Scheduler.SqlEntities.Schedule;

public class Schedule : Entity
{
    public required string Name { get; set; }
    
    public List<SchedulePage> Pages { get; set; } = [];
}