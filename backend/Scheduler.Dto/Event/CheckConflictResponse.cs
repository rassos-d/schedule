namespace Scheduler.Dto.Event;

public class CheckConflictResponse
{
    public string Message { get; set; }
    
    public List<Guid> ConflictEventIds { get; set; }
}