namespace Scheduler.Dto;

public class UpdateEntityNameRequest
{
    public required Guid Id { get; init; }
    
    public required string Name { get; init; }
}