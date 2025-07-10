namespace Scheduler.Dto;

public record CreateEntityWithNameRequest
{
    public required string Name { get; init; }
}