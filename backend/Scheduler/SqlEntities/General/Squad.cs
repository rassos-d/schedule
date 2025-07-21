using Scheduler.Dto.Constants;
using Scheduler.SqlEntities.Base;
using Scheduler.SqlEntities.Plan;

namespace Scheduler.SqlEntities.General;

public class Squad : Entity
{
    public required string Name { get; set; }
    
    public StudyYear? StudyYear { get; set; }
    
    public Teacher? Daddy {  get; set; }
    
    public Audience? FixedAudience {  get; set; }

    public Direction? Direction { get; set; }
}