using Scheduler.Dto.Constants;

namespace Scheduler.Dto.Plan.Lesson;

public class LessonCreateDto
{
    public int Number { get; set; }
    public int HoursCount { get; set; } = 2;
    public LessonType Type { get; set; }
    public Guid ThemeId { get; set; }
    public Guid SubjectId { get; set; }
    public Semester Semester { get; set; }
}