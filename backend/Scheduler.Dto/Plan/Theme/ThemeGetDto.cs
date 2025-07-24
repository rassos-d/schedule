using Scheduler.Dto.Constants;

namespace Scheduler.Dto.Plan.Theme;

public class ThemeGetDto
{
    public required Guid? Id { get; init; }
    public required int? Number { get; init; }

    public Semester Semester { get; init; }
}