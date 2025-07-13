using Scheduler.Entities.Base;
using Scheduler.Dto.Constants;

namespace Scheduler.Entities.Plan;

public class Theme : Entity
{
    public required string Name { get; set; }
    public Guid SubjectId { get; set; }

    public Semester Semester { get; set; }
    
    public List<Lesson> Lessons { get; set; } = [];
}