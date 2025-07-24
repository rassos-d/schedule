using Scheduler.Dto.Constants;

namespace Scheduler.Dto.Plan.Lesson;

public record LessonCreateDto(
    int Number,
    int SelfStudyHours,
    LessonType Type,
    Guid ThemeId,
    Guid SubjectId,
    Semester Semester
    );