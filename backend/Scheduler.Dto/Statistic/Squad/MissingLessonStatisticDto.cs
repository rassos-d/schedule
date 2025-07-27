namespace Scheduler.Dto.Statistic.Squad;

public class MissingLessonStatisticDto
{
    public required Guid LessonId { get; init; }
    public required int LessonNumber { get; init; }
    public required Guid ThemeId { get; init; }
    public required int ThemeNumber { get; init; }
    public required int HoursCount { get; init; }
}