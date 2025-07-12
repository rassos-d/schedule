using Scheduler.Entities.Base;
using Scheduler.Entities.Constants;

namespace Scheduler.Entities.General;

public class Squad : Entity
{
    public required string Name { get; set; }

    public StudyYear? StudyYear { get; set; }
    
    public Guid? DaddyId {  get; set; }
    
    public Guid? FixedAudienceId {  get; set; }

    public Guid? DirectionId { get; set; }
}