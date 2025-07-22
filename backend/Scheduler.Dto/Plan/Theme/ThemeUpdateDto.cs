using Scheduler.Dto.Constants;

namespace Scheduler.Dto.Plan.Theme;

public class ThemeUpdateDto : EntityNameUpdateDto
{
    public int? Number { get; set; }
    
    public Semester? Semester { get; init; }
}