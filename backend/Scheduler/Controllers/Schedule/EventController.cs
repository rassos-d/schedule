using Microsoft.AspNetCore.Mvc;
using Scheduler.DataAccess;
using Scheduler.Dto;
using Scheduler.Entities;

namespace Scheduler.Controllers.Schedule;

[ApiController]
[Route("api/events")]
public class EventController : ControllerBase
{
    private readonly ScheduleRepository _scheduleRepo;

    public EventController(ScheduleRepository scheduleRepo)
    {
        _scheduleRepo = scheduleRepo;
    }

    [HttpGet("find")]
    public IActionResult Find([FromQuery] Guid scheduleId)
    {
        var schedule = _scheduleRepo.GetSchedule(scheduleId);
        if (schedule == null)
        {
            return NotFound();
        }

        return Ok(schedule.Events);
    }
    
    [HttpPost]
    public IActionResult AddEvent([FromBody] Event newEvent)
    {
        var schedule = _scheduleRepo.GetSchedule(newEvent.ScheduleId);
        if (schedule == null)
        {
            return NotFound();
        }

        schedule.Events.Add(newEvent);
        _scheduleRepo.SaveSchedule(schedule);

        return Ok(new SimpleDto<Guid>(schedule.Id));

    }
    
    [HttpPut("{scheduleId}/events/{eventId}")]
    public IActionResult UpdateEvent(Guid scheduleId, Guid eventId, [FromBody] Event updatedEvent)
    {
        var schedule = _scheduleRepo.GetSchedule(scheduleId);
        if (schedule == null)
        {
            return NotFound();
        }

        var existingEvent = schedule.Events.FirstOrDefault(e => e.Id == eventId);
        if (existingEvent == null)
        {
            return NotFound();
        }

        existingEvent.TeacherId = updatedEvent.TeacherId;
        existingEvent.AudienceId = updatedEvent.AudienceId;
        existingEvent.Date = updatedEvent.Date;
        existingEvent.EventNumber = updatedEvent.EventNumber;
        existingEvent.LessonId = updatedEvent.LessonId;
        existingEvent.SquadId = updatedEvent.SquadId;

        _scheduleRepo.SaveSchedule(schedule);
        return NoContent();
    }
    
    [HttpDelete("{scheduleId}/events/{eventId}")]
    public IActionResult DeleteEvent(Guid scheduleId, Guid eventId)
    {
        var schedule = _scheduleRepo.GetSchedule(scheduleId);
        if (schedule == null)
        {
            return NotFound();
        }

        var eventToRemove = schedule.Events.FirstOrDefault(e => e.Id == eventId);
        if (eventToRemove == null)
        {
            return NotFound();
        }

        schedule.Events.Remove(eventToRemove);
        _scheduleRepo.SaveSchedule(schedule);
        return NoContent();
    }
}