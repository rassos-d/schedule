using Scheduler.Entities.Base;

namespace Scheduler.Entities.Plan;

public class Direction : EntityWithName
{
    public List<Subject> Subjects { get; init; } = [];
}