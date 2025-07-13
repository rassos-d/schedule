using Scheduler.Dto.General.Squad;

public class GetEventsByScheduleResponse
{
    public List<GetSquadResponse> Squads { get; set; }

    public List<EventsResponse> NoName { get; set; }
}