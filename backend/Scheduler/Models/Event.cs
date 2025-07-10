using Scheduler.Models.Base;

namespace Scheduler.Models;

public class Event : Entity
{
    public Guid? LessonId { get; set; }
    public Guid? SquadId { get; set; }
    public Guid? TeacherId { get; set; }
    public Guid? AudienceId { get; set; }
    public int? EventNumber { get; set; }
    public DateTime? Date { get; set; }
}