using Microsoft.AspNetCore.Mvc;
using Scheduler.DataAccess;
using Scheduler.Dto;
using Scheduler.Entities.Schedule;

namespace Scheduler.Controllers.Schedule;

using Entities;

[ApiController]
[Route("api/schedules")]
public class ScheduleController : ControllerBase
{
    private readonly ScheduleRepository _scheduleRepo;

    public ScheduleController(ScheduleRepository scheduleRepo)
    {
        _scheduleRepo = scheduleRepo;
    }
    
    [HttpGet("find")]
    public IActionResult Find()
    {
        var schedules = _scheduleRepo.GetAllScheduleInfos();
        return Ok(schedules);
    }
    
    [HttpPost]
    public IActionResult Create([FromBody] EntityWithNameCreateDto dto)
    {
        var schedule = new StudyYearPage { Name = dto.Name };
        _scheduleRepo.SaveSchedule(schedule);
        return Ok(new SimpleDto<Guid>(schedule.Id));
    }

    [HttpPut]
    public IActionResult Update([FromBody] EntityNameUpdateDto dto)
    {
        return Ok();
    }
    
    [HttpDelete("{scheduleId}")]
    public IActionResult DeleteSchedule(Guid scheduleId)
    {
        _scheduleRepo.DeleteSchedule(scheduleId);
        return NoContent();
    }
}