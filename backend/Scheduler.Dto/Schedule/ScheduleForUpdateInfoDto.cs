namespace Scheduler.Dto.Schedule;

public class ScheduleForUpdateInfoDto : ScheduleCreateDto
{
    public required Guid Id { get; init; }
}