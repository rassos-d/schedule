namespace Scheduler.Dto.Schedule;

public class ScheduleGenerationDto
{
    public List<Guid> TeacherIds { get; set; }

    public DayOfWeek DayOfWeek { get; set; }
}