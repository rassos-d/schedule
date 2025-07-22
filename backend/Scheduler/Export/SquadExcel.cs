using Scheduler.Entities;

internal record SquadExcel
{
    public required string Name { get; init; }
    public required string DirectionName { get; init; }
    public required string DaddyName { get; init; }
    public required List<Event> Events { get; init; }
}