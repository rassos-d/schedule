using Microsoft.AspNetCore.Mvc;
using Scheduler.Dto.Constants;
using Scheduler.Entities;
using Scheduler.Services.Events;

namespace Scheduler.Controllers.Schedule;

[ApiController]
[Route("api/events")]
public class EventController : ControllerBase
{
    private readonly EventService _eventService;

    public EventController(EventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet("schedules/{scheduleId::guid}/{studyYear}")]
    public IActionResult Get(Guid scheduleId, StudyYear studyYear)
    {
        var events = _eventService.GetEventsBySchedule(scheduleId, studyYear);
        return Ok(events);
    }
    
    [HttpPost("schedules/{scheduleId::guid}/{studyYear}")]
    public IActionResult AddEvent(Guid scheduleId, StudyYear studyYear, [FromBody] Event newEvent)
    {
        return Ok(_eventService.AddEvent(scheduleId, studyYear, newEvent));
    }
    
    [HttpPut("{eventId}/schedules/{scheduleId}/{studyYear}")]
    public IActionResult UpdateEvent([FromRoute] Guid eventId, [FromRoute] Guid scheduleId, [FromRoute] StudyYear studyYear, [FromBody] Event updatedEvent)
    {
        return Ok(_eventService.UpdateEvent( scheduleId, studyYear, eventId, updatedEvent));
    }
    
    [HttpDelete("{eventId:guid}/schedules/{scheduleId:guid}/{studyYear}/")]
    public IActionResult DeleteEvent(Guid scheduleId, Guid eventId, StudyYear studyYear)
    {
        _eventService.DeleteEvent(scheduleId, studyYear,  eventId);
        return NoContent();
    }
}