namespace Scheduler.Dto.General.Teacher;

public class TeacherCreateDto
{
    public required string Name { get; init; }

    public required string Rank { get; init; }

    public List<VacationPeriod> Vacations { get; set; } = [];
}