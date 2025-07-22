using Scheduler.Dto.Constants;

namespace Scheduler.Dto.Plan.Lesson;

public class LessonGetDto : EntityWithNameGetDto
{
    public required int? Number { get; init; }
    
    public required string? Type { get; init; }
    
    public LessonType? LessonType { get; set; }
}