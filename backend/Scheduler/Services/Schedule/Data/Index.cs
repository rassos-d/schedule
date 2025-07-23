namespace Scheduler.Services.Schedule.Data;

public record Index(int Subject, ValueData<int> Lesson)
{
    public int Subject { get; set; } = Subject;
    public ValueData<int> Lesson { get; set; } = Lesson;
}