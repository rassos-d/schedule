using Scheduler.Entities.Base;

namespace Scheduler.Entities.General;

public class Teacher : Entity
{
    public required string Name { get; set; }

    public required string Rank { get; set; }

    public List<Tuple<DateOnly, DateOnly>> Vacations { get; set; } = [];

    public List<Guid> SubjectIds { get; set; } = [];
}