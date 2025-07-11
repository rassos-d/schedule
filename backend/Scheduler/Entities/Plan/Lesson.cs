using Scheduler.Entities.Base;
using Scheduler.Entities.Constants;

namespace Scheduler.Entities.Plan;

public class Lesson : Entity
{
    public required string Name { get; set; }

    public LessonType Type { get; set; }

    public Guid DirectionId { get; set; }
    
    public Guid SubjectId { get; set; }
    
    public Guid ThemeId { get; set; }
}