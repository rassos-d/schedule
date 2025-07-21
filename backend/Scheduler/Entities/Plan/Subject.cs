using Scheduler.Dto.Constants;
using Scheduler.Entities.Base;

namespace Scheduler.Entities.Plan;

public class Subject : EntityWithName
{
    public Guid DirectionId { get; set; }

    public Semester Semester { get; set; }

    public List<Theme> Themes { get; set; } = [];
}