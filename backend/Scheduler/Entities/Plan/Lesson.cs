using Scheduler.Entities.Base;
using Scheduler.Entities.Constants;

namespace Scheduler.Entities.Plan;

public class Lesson : EntityWithName
{
    public int Number { get; set; }

    public int SelfStudyHours { get; set; }

    public LessonType Type { get; set; }
    
    public Guid SubjectId { get; set; }
    
    public Guid ThemeId { get; set; }
}