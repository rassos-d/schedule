using Microsoft.AspNetCore.Mvc;
using Scheduler.Dto;
using Scheduler.Dto.Schedule;
using Scheduler.Entities.Constants;
using Scheduler.Services.Schedule;

namespace Scheduler.Controllers.Schedule;

[ApiController]
[Route("api/schedules")]
public class ScheduleController(ScheduleService service) : ControllerBase
{
    [HttpGet("find")]
    public IActionResult Find()
    {
        var schedules = service.Find();
        return Ok(schedules);
    }

    [HttpGet("{scheduleId:guid}/pages/{studyYear:int}")]
    public IActionResult GetPage(Guid scheduleId, int studyYear)
    {
        var page = service.GetPage(scheduleId, studyYear);
        return Ok(page);
    }
    
    [HttpPost]
    public IActionResult Create([FromBody] ScheduleCreateDto dto)
    {
        var id = service.Create(dto);
        return Ok(new SimpleDto<Guid>(id));
    }

    [HttpPut]
    public IActionResult Update([FromBody] EntityNameUpdateDto dto)
    {
        service.Update(dto);
        return Ok();
    }
    
    [HttpDelete("{scheduleId}")]
    public IActionResult DeleteSchedule(Guid scheduleId)
    {
        service.Delete(scheduleId);
        return NoContent();
    }
}