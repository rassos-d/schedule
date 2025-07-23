using Scheduler.Entities.Base;

namespace Scheduler.Entities.Plan;

public class Subject : EntityWithName
{
    public string? ShortName { get; set; }
    public Guid DirectionId { get; set; }

    public List<Theme> Themes { get; set; } = [];
}