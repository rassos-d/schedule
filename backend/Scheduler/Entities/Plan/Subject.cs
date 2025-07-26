using Scheduler.Entities.Base;

namespace Scheduler.Entities.Plan;

public class Subject : EntityWithName
{
    public Guid DirectionId { get; set; }

    public List<Theme> Themes { get; set; } = [];

    public string? Color { get; set; }
}