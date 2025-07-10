using Scheduler.Models.Base;
using Scheduler.Models.Constants;

namespace Scheduler.Models.Plan;

public class Lesson : Entity
{
    public required string Name { get; set; }

    public LessonType Type { get; set; }

    public Guid ThemeId { get; set; }
}