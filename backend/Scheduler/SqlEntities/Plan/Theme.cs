using Scheduler.SqlEntities.Base;

namespace Scheduler.SqlEntities.Plan;

public class Theme : Entity
{
    public required string Name { get; set; }

    public int Number { get; set; }

    public required Subject Subject { get; set; } = null!;
    
    public List<Lesson> Lessons { get; set; } = [];
}