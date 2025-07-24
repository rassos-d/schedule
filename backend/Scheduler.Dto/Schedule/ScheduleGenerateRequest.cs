namespace Scheduler.Dto.Schedule;

public class ScheduleGenerateRequest
{
    public List<Guid> TeacherIds { get; set; }
    public List<Guid> AudienceIds { get; set; }
    public List<Guid> SquadIds { get; set; }
}