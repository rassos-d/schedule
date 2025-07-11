using Scheduler.Entities.Base;

namespace Scheduler.Entities.Plan;

public class Theme : Entity
{
    public required string Name { get; set; }
    
    public Guid DirectionId { get; set; }
    public Guid SubjectId { get; set; }
    
    public List<Lesson> Lessons { get; set; } = [];
}