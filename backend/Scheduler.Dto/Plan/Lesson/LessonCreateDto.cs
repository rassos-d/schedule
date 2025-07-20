using Scheduler.Entities.Constants;

namespace Scheduler.Dto.Lesson;

public record LessonCreateDto(
    string Name,
    int Number,
    int SelfStudyHours,
    LessonType Type,
    Guid ThemeId,
    Guid SubjectId
    );