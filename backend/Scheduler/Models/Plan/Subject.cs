using Scheduler.Models.Base;

namespace Scheduler.Models.Plan;

public class Subject : Entity
{
    public required string Name { get; set; }

    public Guid DirectionId { get; set; }
}