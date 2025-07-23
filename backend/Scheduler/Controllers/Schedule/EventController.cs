using Microsoft.AspNetCore.Mvc;
using Scheduler.Dto.Constants;
using Scheduler.Entities;
using Scheduler.Services.Schedule;

namespace Scheduler.Controllers.Schedule;

[ApiController]
[Route("api/events")]
public class EventController(EventService eventService) : ControllerBase
{
    [HttpGet("schedules/{scheduleId::guid}/{studyYear}")]
    public IActionResult Get(Guid scheduleId, StudyYear studyYear)
    {
        var events = eventService.GetEventsBySchedule(scheduleId, studyYear);
        return Ok(events);
    }
    
    [HttpPost("schedules/{scheduleId::guid}/{studyYear}")]
    public IActionResult AddEvent(Guid scheduleId, StudyYear studyYear, [FromBody] Event newEvent)
    {
        return Ok(eventService.AddEvent(scheduleId, studyYear, newEvent));
    }
    
    [HttpPut("{eventId}/schedules/{scheduleId}/{studyYear}")]
    public IActionResult UpdateEvent([FromRoute] Guid eventId, [FromRoute] Guid scheduleId, [FromRoute] StudyYear studyYear, [FromBody] Event updatedEvent)
    {
        return Ok(eventService.UpdateEvent( scheduleId, studyYear, eventId, updatedEvent));
    }
    
    [HttpDelete("{eventId:guid}/schedules/{scheduleId:guid}/{studyYear}/")]
    public IActionResult DeleteEvent(Guid scheduleId, Guid eventId, StudyYear studyYear)
    {
        eventService.DeleteEvent(scheduleId, studyYear,  eventId);
        return NoContent();
    }
}