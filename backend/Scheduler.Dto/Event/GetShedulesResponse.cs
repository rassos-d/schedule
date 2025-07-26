using Scheduler.Dto.General.Squad;
using Scheduler.Dto.Plan.Subject;

namespace Scheduler.Dto.Event;

public class GetEventsByScheduleResponse
{
    public Guid ScheduleId { get; init; }

    public string Name { get; init; }

    public string DayOfWeek { get; set; }

    public int? Semester { get; init; }
    public List<GetSquadResponse> Squads { get; init; } = [];

    public List<EventsResponse> NoName { get; init; } = [];

    public CheckConflictResponse Conflicts { get; init; }

    public List<SubjectColorDto> SubjectColors { get; set; }
}