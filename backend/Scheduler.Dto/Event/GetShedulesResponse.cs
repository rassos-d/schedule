using Scheduler.Dto.Event;
using Scheduler.Dto.General.Squad;

public class GetEventsByScheduleResponse
{
    public Guid ScheduleId { get; set; }

    public string Name { get; set; }
    public List<GetSquadResponse> Squads { get; set; }

    public List<EventsResponse> NoName { get; set; }
    
    public CheckConflictResponse Conflicts { get; set; }
}