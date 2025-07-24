using Scheduler.Dto.Constants;

namespace Scheduler.Dto.Plan.Lesson;

public class LessonUpdateDto
{
    public required Guid Id { get; init; }
    
    public int? Number { get; set; }
    
    public LessonType Type { get; set; }

    public int HoursCount { get; set; } = 2;
    
    public Semester? Semester { get; set; }
}