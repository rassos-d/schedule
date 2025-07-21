using Scheduler.Dto.Constants;
using Scheduler.Entities.Base;

namespace Scheduler.Entities.General;

public class Squad : EntityWithName
{
    public StudyYear? StudyYear { get; set; }
    
    public Guid? DaddyId {  get; set; }
    
    public Guid? FixedAudienceId {  get; set; }

    public Guid? DirectionId { get; set; }
}