using Scheduler.Dto.Constants;

namespace Scheduler.Dto.Plan.Theme;

public record ThemeCreateDto(int Number, Guid SubjectId, Semester Semester);