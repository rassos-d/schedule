using Scheduler.Dto.General.Squad;
using Scheduler.Dto.Plan.Lesson;

namespace Scheduler.Dto.Event;

public class EventResponse
{
    public Guid Id { get; set; }
    
    public Guid ScheduleId { get; set; }

    public LessonGetDto Lesson { get; set; }
    
    public GetSquadResponse Squad { get; set; }
}