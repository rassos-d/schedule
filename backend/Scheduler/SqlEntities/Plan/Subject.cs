using Scheduler.Dto.Constants;
using Scheduler.SqlEntities.Base;

namespace Scheduler.SqlEntities.Plan;

public class Subject : Entity
{
    public required string Name { get; set; }

    public Direction Direction { get; set; } = null!;

    public Semester Semester { get; set; }

    public List<Theme> Themes { get; set; } = [];
}