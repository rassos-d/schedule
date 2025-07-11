using Microsoft.AspNetCore.Mvc;
using Scheduler.DataAccess;
using Scheduler.DataAccess.Plan;
using Scheduler.Dto;
using Scheduler.Entities;

namespace Scheduler.Controllers.Schedule;

[ApiController]
[Route("api/events")]
public class EventController : ControllerBase
{
    private readonly ScheduleRepository _scheduleRepo;
    private readonly GeneralRepository _generalRepo;
    private readonly PlanRepository _planRepository;
    private readonly GeneralRepository generalRepository;
    public PlanRepository planRepository;

    public EventController(
        ScheduleRepository scheduleRepo,
        PlanRepository planRepository,
        GeneralRepository generalRepository)
    {
        _scheduleRepo = scheduleRepo;
        this.planRepository = planRepository;
        this.generalRepository = generalRepository;
    }

    [HttpGet]
    public IActionResult GetAllSchedules()
    {
        return Ok(_scheduleRepo.GetAllScheduleInfos());
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
    public IActionResult AddEvent(Guid scheduleId, [FromBody] Event newEvent)
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

    [HttpDelete("{scheduleId}")]
    public IActionResult DeleteSchedule(Guid scheduleId)
    {
        _scheduleRepo.DeleteSchedule(scheduleId);
        return NoContent();
    }
}