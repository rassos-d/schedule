using Scheduler.Dto.Constants;

namespace Scheduler.Dto.Plan.Lesson;

public class LessonInfo
{
    public required Guid Id { get; init; }
    
    public required int? Number { get; init; }
    
    public int? ThemeNumber { get; init; }
    
    public required string? Type { get; init; }
    
    public LessonType? LessonType { get; set; }
    
    public Semester Semester { get; set; }
    
    public Guid SubjectId { get; set; }
    
    public Guid ThemeId { get; set; }
}