using Scheduler.Entities.Base;

namespace Scheduler.Entities.Plan;

public class Subject : Entity
{
    public required string Name { get; set; }

    public Guid DirectionId { get; set; }

    public List<Theme> Themes { get; set; } = [];
}