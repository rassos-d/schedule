using Scheduler.Dto.Constants;
using Scheduler.Entities.Base;

namespace Scheduler.Entities.Plan;

public class Theme : Entity
{
    public int Number { get; set; }
    
    public Semester Semester { get; set; }

    public Guid SubjectId { get; set; }
    
    public List<Lesson> Lessons { get; set; } = [];
}