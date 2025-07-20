using Scheduler.Entities.Base;
using Scheduler.Dto.Constants;

namespace Scheduler.Entities.Plan;

public class Theme : Entity
{
    public string Name { get; set; }

    public int Number { get; set; }

    public Guid SubjectId { get; set; }
    
    public List<Lesson> Lessons { get; set; } = [];
}