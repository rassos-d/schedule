using Scheduler.SqlEntities.Base;

namespace Scheduler.SqlEntities.Plan;

public class Direction : Entity
{
    public required string Name { get; set; }

    public List<Subject> Subjects { get; init; } = [];
}