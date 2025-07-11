namespace Scheduler.Entities;

public class Schedule
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public List<Event> Events { get; set; } = [];
}