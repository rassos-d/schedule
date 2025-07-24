namespace Scheduler.Dto.Plan.Theme;

public class ThemeUpdateDto
{
    public required Guid Id { get; init; }
    
    public int? Number { get; set; }
}