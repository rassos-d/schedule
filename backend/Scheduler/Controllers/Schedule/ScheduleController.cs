using Microsoft.AspNetCore.Mvc;
using Scheduler.Dto;
using Scheduler.Dto.Constants;
using Scheduler.Dto.Schedule;
using Scheduler.Entities.Constants;
using Scheduler.Services.Events;
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

    [HttpGet("{scheduleId:guid}/pages/{studyYear}")]
    public IActionResult GetPage(Guid scheduleId, StudyYear studyYear)
    {
        var response = service.GetPage(scheduleId, studyYear);
        return Ok(response);
    }

    [HttpGet("{scheduleId:guid}/study-years")]
    public IActionResult GetStudyYears(Guid scheduleId)
    {
        return Ok(service.GetStudyYears(scheduleId));
    }
    
    [HttpPost]
    public IActionResult Create([FromBody] ScheduleCreateDto dto)
    {
        var id = service.Create(dto);
        return Ok(new SimpleDto<Guid>(id));
    }
   
    [HttpPost("{scheduleId:guid}/excel")]
    public IActionResult SaveExcel(Guid scheduleId)
    {
        var name = service.GetName(scheduleId);
        using var result = service.ExportExcel(scheduleId);

        return File(result, "application/octet-stream", $"{name}.xlsx");
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