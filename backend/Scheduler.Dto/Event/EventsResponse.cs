using Scheduler.Dto;
using Scheduler.Dto.Plan.Lesson;
using Scheduler.Dto.Plan.Theme;
using Scheduler.Entities.Constants;

public class EventsResponse
{
    public Guid Id { get; set; }
    public EntityWithNameGetDto? Teacher { get; set; }

    public EntityWithNameGetDto? Audience { get; set; }

    public LessonGetDto? Lesson { get; set; }

    public EntityWithNameGetDto? Squad { get; set; }
    
    public ThemeGetDto? Theme { get; set; }
    
    public EntityWithNameGetDto? Subject { get; set; }
    
    public LessonType? LessonType { get; set; }

    public int? Number { get; set; }

    public DateOnly? Date { get; set; }
}