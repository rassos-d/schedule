using Microsoft.AspNetCore.Mvc;
using Scheduler.Data;
using Scheduler.Models;

namespace Scheduler.Api;

[ApiController]
[Route("api/schedules")]
public class ScheduleController : ControllerBase
{
    private readonly ScheduleRepository _scheduleRepo;

    public ScheduleController(ScheduleRepository scheduleRepo)
    {
        _scheduleRepo = scheduleRepo;
    }
    
    [HttpGet]
    public IActionResult GetAllSchedules()
    {
        return Ok(_scheduleRepo.GetAllScheduleInfos());
    }
    
    [HttpGet("{scheduleId}/events")]
    public IActionResult GetScheduleEvents(Guid scheduleId)
    {
        var schedule = _scheduleRepo.GetSchedule(scheduleId);
        if (schedule == null)
        {
            return NotFound();
        }

        return Ok(schedule.Events);
    }
    
    [HttpPost("{scheduleId}/events")]
    public IActionResult AddEvent(Guid scheduleId, [FromBody] Event newEvent)
    {
        var schedule = _scheduleRepo.GetSchedule(scheduleId);
        if (schedule == null)
        {
            return NotFound();
        }

        newEvent.Id = Guid.NewGuid();
        schedule.Events.Add(newEvent);
        _scheduleRepo.SaveSchedule(schedule);

        return CreatedAtAction(nameof(GetScheduleEvents),
            new { scheduleId }, newEvent);

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
    
    [HttpPost]
    public IActionResult Create([FromBody] string name)
    {
        var newSchedule = new Schedule { Name = name };
        _scheduleRepo.SaveSchedule(newSchedule);
        return CreatedAtAction(nameof(GetAllSchedules), newSchedule);
    }
    
    [HttpDelete("{scheduleId}")]
    public IActionResult DeleteSchedule(Guid scheduleId)
    {
        if (!_scheduleRepo.DeleteSchedule(scheduleId))
        {
            return NotFound();
        }

        return NoContent();
    }
}