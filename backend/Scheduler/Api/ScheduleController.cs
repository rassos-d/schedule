// Controllers/ScheduleController.cs
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly ScheduleRepository _scheduleRepo;
    private readonly GeneralRepository _generalRepo;

    public ScheduleController(ScheduleRepository scheduleRepo, GeneralRepository generalRepo)
    {
        _scheduleRepo = scheduleRepo;
        _generalRepo = generalRepo;
    }

    /// <summary>
    /// Get all schedules with basic info
    /// </summary>
    [HttpGet]
    public IActionResult GetAllSchedules()
    {
        return Ok(_scheduleRepo.GetAllScheduleInfos());
    }

    /// <summary>
    /// Get all events in specific schedule
    /// </summary>
    /// <param name="scheduleId">Schedule identifier</param>
    [HttpGet("{scheduleId}/events")]
    public IActionResult GetScheduleEvents(Guid scheduleId)
    {
        var schedule = _scheduleRepo.GetSchedule(scheduleId);
        if (schedule == null) return NotFound();
        
        return Ok(schedule.Events);
    }

    /// <summary>
    /// Create new event in schedule
    /// </summary>
    /// <param name="scheduleId">Schedule identifier</param>
    /// <param name="newEvent">Event data</param>
    [HttpPost("{scheduleId}/events")]
    public IActionResult AddEvent(Guid scheduleId, [FromBody] Event newEvent)
    {
        var schedule = _scheduleRepo.GetSchedule(scheduleId);
        if (schedule == null) return NotFound();

        newEvent.Id = Guid.NewGuid();
        schedule.Events.Add(newEvent);
        _scheduleRepo.SaveSchedule(schedule);
        
        return CreatedAtAction(nameof(GetScheduleEvents), 
            new { scheduleId }, newEvent);
    }

    /// <summary>
    /// Update existing event
    /// </summary>
    /// <param name="scheduleId">Schedule identifier</param>
    /// <param name="eventId">Event identifier</param>
    /// <param name="updatedEvent">Updated event data</param>
    [HttpPut("{scheduleId}/events/{eventId}")]
    public IActionResult UpdateEvent(Guid scheduleId, Guid eventId, [FromBody] Event updatedEvent)
    {
        var schedule = _scheduleRepo.GetSchedule(scheduleId);
        if (schedule == null) return NotFound();

        var existingEvent = schedule.Events.FirstOrDefault(e => e.Id == eventId);
        if (existingEvent == null) return NotFound();

        // Update allowed fields only
        existingEvent.TeacherId = updatedEvent.TeacherId;
        existingEvent.AudienceId = updatedEvent.AudienceId;
        existingEvent.Date = updatedEvent.Date;
        existingEvent.EventNumber = updatedEvent.EventNumber;
        existingEvent.LessonId = updatedEvent.LessonId;
        existingEvent.SquadId = updatedEvent.SquadId;

        _scheduleRepo.SaveSchedule(schedule);
        return NoContent();
    }

    /// <summary>
    /// Delete event from schedule
    /// </summary>
    /// <param name="scheduleId">Schedule identifier</param>
    /// <param name="eventId">Event identifier</param>
    [HttpDelete("{scheduleId}/events/{eventId}")]
    public IActionResult DeleteEvent(Guid scheduleId, Guid eventId)
    {
        var schedule = _scheduleRepo.GetSchedule(scheduleId);
        if (schedule == null) return NotFound();

        var eventToRemove = schedule.Events.FirstOrDefault(e => e.Id == eventId);
        if (eventToRemove == null) return NotFound();

        schedule.Events.Remove(eventToRemove);
        _scheduleRepo.SaveSchedule(schedule);
        return NoContent();
    }

    /// <summary>
    /// Create new schedule
    /// </summary>
    /// <param name="scheduleName">Name for new schedule</param>
    [HttpPost]
    public IActionResult CreateSchedule([FromBody] ScheduleCreateRequest request)
    {
        var newSchedule = new Schedule { Name = request.Name };
        _scheduleRepo.SaveSchedule(newSchedule);
        return CreatedAtAction(nameof(GetAllSchedules), newSchedule);
    }

    /// <summary>
    /// Delete schedule
    /// </summary>
    /// <param name="scheduleId">Schedule identifier</param>
    [HttpDelete("{scheduleId}")]
    public IActionResult DeleteSchedule(Guid scheduleId)
    {
        if (!_scheduleRepo.DeleteSchedule(scheduleId))
            return NotFound();
            
        return NoContent();
    }
}

public class ScheduleCreateRequest
{
    public string Name { get; set; }
}