public class GetSquadResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Dictionary<DateOnly, List<EventResponse>> MyProperty { get; set; }
}