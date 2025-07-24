using Scheduler.Dto.General.Squad;

namespace Scheduler.Dto.Event;

public class GetEventsByScheduleResponse
{
    public Guid ScheduleId { get; init; }

    public string Name { get; init; }

    public int? Semester { get; init; }
    public List<GetSquadResponse> Squads { get; init; } = [];

    public List<EventsResponse> NoName { get; init; } = [];

    public CheckConflictResponse Conflicts { get; init; }
}