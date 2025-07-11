namespace Scheduler.Entities.Plan;

public class Theme
{
    public required string Name { get; set; }
    public Guid SubjectId { get; set; }
    
    public List<Lesson> Lessons { get; set; } = [];
}