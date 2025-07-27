using Scheduler.Dto;
using Scheduler.Dto.General.Teacher;
using Scheduler.Entities.Base;

namespace Scheduler.Entities.General;

public class Teacher : EntityWithName
{
    public required string Rank { get; set; }

    public List<VacationPeriod> Vacations { get; set; } = [];

    public List<Guid> SubjectIds { get; set; } = [];
}