using Scheduler.Entities.Base;

namespace Scheduler.Entities.Plan;

public class Direction : Entity
{
    public required string Name { get; set; }

    public List<Subject> Subjects { get; set; } = [];
}