namespace Scheduler.Entities.Schedule;

public class ScheduleInfo(Guid id, string name)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
}