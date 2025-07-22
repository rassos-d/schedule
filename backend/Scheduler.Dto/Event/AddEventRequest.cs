namespace Scheduler.Dto.Event;

public class AddEventRequest : CheckConflictResponse
{
    public AddEventRequest(Guid id, CheckConflictResponse response)
    {
        Id = id;
        ConflictEventIds = response.ConflictEventIds;
        Message = response.Message;
    }
    
    public Guid Id { get; set; }
}