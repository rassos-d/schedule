using Scheduler.Entities.Constants;
using Scheduler.SqlEntities.Base;

namespace Scheduler.SqlEntities.Plan;

public class Lesson : Entity
{
    public required string Name { get; set; }

    public int Number { get; set; }

    public int SelfStudyHours { get; set; }
    
    public LessonType Type { get; set; }

    public Subject Subject { get; set; } = null!;

    public Theme Theme { get; set; } = null!;
}