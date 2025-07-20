using Scheduler.Dto.Constants;

namespace Scheduler.Dto.General.Squad;

public class SquadUpdateDto
{
    public required Guid Id { get; init; }
    
    public string? Name {  get; init; }
    
    public SimpleDto<StudyYear?>? StudyYear { get; set; }
    
    public SimpleDto<Guid?>? DaddyId {  get; set; }
    
    public SimpleDto<Guid?>? FixedAudienceId {  get; set; }

    public SimpleDto<Guid?>? DirectionId { get; set; }
}