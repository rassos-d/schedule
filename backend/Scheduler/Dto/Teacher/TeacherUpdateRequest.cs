namespace Scheduler.Dto.Teacher;

public class TeacherUpdateRequest
{
    public required string Name { get; init; }

    public required string Rank { get; init; }

    public List<Tuple<DateTime, DateTime>> Vacations { get; init; } = [];

    public List<Guid> SubjectIds { get; init; } = [];
}