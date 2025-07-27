namespace Scheduler.Dto.Statistic.Teacher;

public class TeacherStatisticDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Rank { get; init; }
    public required int HoursCount { get; init; }
    public required int SubjectsCount { get; init; }
}