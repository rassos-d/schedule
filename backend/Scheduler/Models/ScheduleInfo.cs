namespace Scheduler.Models;

public class ScheduleInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public int Semester { get; set; }

    public ScheduleInfo(Guid id, string name, int semester)
    {
        Id = id;
        Name = name;
    }
}