namespace Scheduler.Dto;

public class EntityNameUpdateDto
{
    public required Guid Id { get; init; }
    
    public required string Name { get; init; }
}