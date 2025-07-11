using Scheduler.Entities.Base;

namespace Scheduler.Entities.General;

public class Squad : Entity
{
    public required string Name { get; set; }

    public int StudyYear { get; set; }

    public int SemesterNumber { get; set; }

    public Guid DirectionId { get; set; }
}