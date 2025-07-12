using Scheduler.Dto.General.Squad;

public class GetScheduleResponse
{
    public string Name { get; set; }

    public List<GetSquadResponse> Squads { get; set; }

    public List<EventsResponse> NoName { get; set; }
}