using Scheduler.Dto.General.Squad;
using Scheduler.Dto.Lesson;

namespace Scheduler.Dto.Event;

public class EventResponse
{
    public Guid Id { get; set; }
    
    public Guid ScheduleId { get; set; }

    public LessonResponse Lesson { get; set; }
    
    public GetSquadResponse Squad { get; set; }
}