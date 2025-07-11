using Scheduler.Entities.Base;

namespace Scheduler.Entities;

public class Event : Entity
{
    public Guid ScheduleId { get; init; }
    public Guid? LessonId { get; set; }
    public Guid? SquadId { get; set; }
    public Guid? TeacherId { get; set; }
    public Guid? AudienceId { get; set; }
    public int? EventNumber { get; set; }
    public DateTime? Date { get; set; }
}