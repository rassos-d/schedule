using Scheduler.Entities.Constants;

namespace Scheduler.Dto.Lesson;

public record LessonCreateDto(
    string Name,
    LessonType Type,
    Guid ThemeId,
    Guid SubjectId
    );