namespace Scheduler.Dto;

public class EntityNameUpdateDto
{
    public required Guid Id { get; init; }
    
    public string? Name { get; init; }
}