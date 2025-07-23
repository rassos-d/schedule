namespace Scheduler.Services.Schedule.Data;

public record ValueData<T>(T Value)
{
    public T Value { get; set; } = Value;
}