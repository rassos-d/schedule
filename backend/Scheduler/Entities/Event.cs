using Scheduler.Entities.Base;
using Scheduler.Entities.Constants;

namespace Scheduler.Entities;

public class Event : Entity
{
    public Guid ScheduleId { get; set; }

    public Guid? SubjectId { get; set; }
    
    public Guid? ThemeId { get; set; }

    public LessonType LessonType { get; set; }
    public Guid? LessonId { get; set; }
    public Guid? SquadId { get; set; }
    public Guid? TeacherId { get; set; }
    public Guid? AudienceId { get; set; }
    public int? Number { get; set; }
    public DateOnly? Date { get; set; }
}