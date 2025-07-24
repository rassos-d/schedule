using Scheduler.Dto.Constants;
using Scheduler.Entities.Base;

namespace Scheduler.Entities.Plan;

public class Lesson : Entity
{
    public int Number { get; set; }

    public LessonType Type { get; set; }
    
    public Guid SubjectId { get; set; }
    
    public Guid ThemeId { get; set; }
    
    public Semester Semester { get; set; }
}