namespace Scheduler.Dto.General.Squad;

public class GetSquadResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    
    public string? TeacherName { get; set; }
    
    public string? DirectionName { get; set; }
    public Dictionary<DateOnly, List<EventsResponse>> Events { get; set; }
}