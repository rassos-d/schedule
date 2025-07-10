using Scheduler.Models.Base;

namespace Scheduler.Models.General;

public class Teacher : Entity
{
    public required string Name { get; set; }

    public required string Rank { get; set; }

    public List<VacationPeriod> Vacations { get; set; } = [];

    public List<Guid> SubjectIds { get; set; } = [];
}