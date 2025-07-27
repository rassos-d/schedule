namespace Scheduler.Dto.Base;

public class EntityWithNameGetDto
{
    public required Guid? Id { get; init; }
    public required string? Name { get; init; }
}