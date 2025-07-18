namespace Scheduler.Dto.General.Teacher;

public class TeacherUpdateDto
{
    public required Guid  Id { get; init; }
    
    public string? Name { get; init; }

    public string? Rank { get; init; }

    public List<Tuple<DateOnly, DateOnly>>? Vacations { get; init; } = [];

    public List<Guid>? SubjectIds { get; init; } = [];
}