using Scheduler.Entities.Constants;
using Scheduler.SqlEntities.Base;
using Scheduler.SqlEntities.General;
using Scheduler.SqlEntities.Plan;

namespace Scheduler.SqlEntities.Schedule;

public class Event : Entity
{
    public required SchedulePage Page { get; set; }

    public Subject? Subject { get; set; }

    public Theme? Theme { get; set; }

    public LessonType LessonType { get; set; }
    
    public Lesson? LessonId { get; set; }
    public Squad? SquadId { get; set; }
    public Teacher? TeacherId { get; set; }
    public Audience? AudienceId { get; set; }
    public int? Number { get; set; }
    public DateOnly? Date { get; set; }
}