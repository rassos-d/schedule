using Scheduler.SqlEntities.Base;

namespace Scheduler.SqlEntities.General;

public class Audience : Entity
{
    public required string Name { get; set; }
}