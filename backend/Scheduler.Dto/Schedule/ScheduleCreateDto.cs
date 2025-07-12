namespace Scheduler.Dto.Schedule;

public class ScheduleCreateDto
{
    public required string Name { get; init; }

    public required List<SchedulePageCreateDto> Pages { get; init; } = [];
}