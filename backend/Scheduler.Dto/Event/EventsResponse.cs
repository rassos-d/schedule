using Scheduler.Dto;
using Scheduler.Entities.Constants;

public class EventsResponse
{
    public Guid Id { get; set; }
    public EntityNameResponse? Teacher { get; set; }

    public EntityNameResponse? Audience { get; set; }

    public EntityNameResponse? Lesson { get; set; }

    public EntityNameResponse? Squad { get; set; }
    
    public EntityNameResponse? Theme { get; set; }
    
    public EntityNameResponse? Subject { get; set; }
    
    public LessonType? LessonType { get; set; }

    public int? Number { get; set; }

    public DateOnly? Date { get; set; }
}