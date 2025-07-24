using Microsoft.AspNetCore.Mvc;
using Scheduler.Entities;
using Scheduler.Services.Events;

namespace Scheduler.Controllers.Schedule;

[ApiController]
[Route("api/events")]
public class EventController(EventService eventService) : ControllerBase
{
    [HttpGet("schedules/{scheduleId::guid}/{dayOfWeek}")]
    public IActionResult Get(Guid scheduleId, DayOfWeek dayOfWeek)
    {
        var events = eventService.GetEventsBySchedule(scheduleId, dayOfWeek);
        return Ok(events);
    }
    
    [HttpPost("schedules/{scheduleId::guid}/{dayOfWeek}")]
    public IActionResult AddEvent(Guid scheduleId, DayOfWeek dayOfWeek, [FromBody] Event newEvent)
    {
        return Ok(eventService.AddEvent(scheduleId, dayOfWeek, newEvent));
    }
    
    [HttpPut("{eventId}/schedules/{scheduleId}/{dayOfWeek}")]
    public IActionResult UpdateEvent([FromRoute] Guid eventId, [FromRoute] Guid scheduleId, [FromRoute] DayOfWeek dayOfWeek, [FromBody] Event updatedEvent)
    {
        return Ok(eventService.UpdateEvent( scheduleId, dayOfWeek, eventId, updatedEvent));
    }
    
    [HttpDelete("{eventId:guid}/schedules/{scheduleId:guid}/{dayOfWeek}/")]
    public IActionResult DeleteEvent(Guid scheduleId, Guid eventId, DayOfWeek dayOfWeek)
    {
        eventService.DeleteEvent(scheduleId, dayOfWeek,  eventId);
        return NoContent();
    }
}