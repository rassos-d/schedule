namespace Scheduler.Dto.Base;

public record EntityWithNameCreateDto
{
    public required string Name { get; init; }
}