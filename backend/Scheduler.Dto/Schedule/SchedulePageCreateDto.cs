using Scheduler.Entities.Constants;

namespace Scheduler.Dto.Schedule;

public class SchedulePageCreateDto
{
    public required StudyYear StudyYear { get; init; }

    public required List<Guid> Squads { get; init; } = [];
    
    public required DateOnly Start { get; init; }
    
    public required DateOnly End { get; init; }
}