using Scheduler.Dto.Constants;

namespace Scheduler.Dto.Plan.Theme;

public class ThemeGetDto : EntityWithNameGetDto
{
    public required int? Number { get; init; }

    public Semester Semester { get; init; }
}