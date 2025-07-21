namespace Scheduler.Dto.General.Squad;

public class GetSquadResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    
    public EntityNameResponse? Daddy { get; set; }
    
    public EntityNameResponse? Direction { get; set; }
    
    public EntityNameResponse? Audience { get; set; }
    public Dictionary<DateOnly, List<EventsResponse>> Events { get; set; }
}