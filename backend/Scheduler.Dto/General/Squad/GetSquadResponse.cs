namespace Scheduler.Dto.General.Squad;

public class GetSquadResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Dictionary<DateTime, List<EventsResponse>> Events { get; set; }
}