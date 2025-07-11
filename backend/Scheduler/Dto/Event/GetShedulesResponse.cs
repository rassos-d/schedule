public class GetScheduleResponse
{
    public string Name { get; set; }

    public List<GetSquadResponse> Squads { get; set; }

    public List<EventResponse> NoName { get; set; }
}