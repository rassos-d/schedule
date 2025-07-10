public class Schedule
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public List<Event> Events { get; set; } = new();
}