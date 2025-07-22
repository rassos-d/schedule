using Scheduler.Dto.Constants;

namespace Scheduler.Dto.Plan.Theme;

public record ThemeCreateDto(string Name, int Number, Guid SubjectId, Semester Semester);