namespace Scheduler.Dto;

public record EntityWithNameCreateDto
{
    public required string Name { get; init; }
}