namespace Scheduler.Dto.Statistic.Squad;

public class SubjectStatisticDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public int PlannedHours  { get; init; }
    public int CompletedHours  { get; init; }
    public List<MissingLessonStatisticDto> MissingLessons { get; init; } = [];
}