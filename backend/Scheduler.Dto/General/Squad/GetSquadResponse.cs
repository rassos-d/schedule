using Scheduler.Dto.Base;

namespace Scheduler.Dto.General.Squad;

public class GetSquadResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    
    public EntityWithNameGetDto? Daddy { get; set; }
    
    public EntityWithNameGetDto? Direction { get; set; }
    
    public EntityWithNameGetDto? Audience { get; set; }
    public Dictionary<DateOnly, List<EventsResponse>> Events { get; set; }
}