namespace Scheduler.Models.Plan;

public class Theme
{
    public required string Name { get; set; }
    public Guid SubjectId { get; set; }
}