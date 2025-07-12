namespace Scheduler.Models;

public class ScheduleInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public ScheduleInfo(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}