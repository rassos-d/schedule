using Scheduler.Entities.Constants;

public class EventsResponse
{
    public Guid Id { get; set; }
    public string? TeacherName { get; set; }

    public string? AudienceName { get; set; }

    public string? LessonName { get; set; }

    public string? SquadName { get; set; }
    
    public string? ThemeName { get; set; }
    
    public string? SubjectName { get; set; }
    
    public LessonType? LessonType { get; set; }

    public int? Number { get; set; }

    public DateOnly? Date { get; set; }
}