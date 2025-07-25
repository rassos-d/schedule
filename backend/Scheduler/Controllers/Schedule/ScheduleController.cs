using Microsoft.AspNetCore.Mvc;
using Scheduler.Dto;
using Scheduler.Dto.Constants;
using Scheduler.Dto.Schedule;
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

    [HttpGet("{scheduleId:guid}/pages/{dayOfWeek}")]
    public IActionResult GetPage(Guid scheduleId, DayOfWeek dayOfWeek)
    {
        var response = service.GetPage(scheduleId, dayOfWeek);
        return Ok(response);
    }

    [HttpGet("{scheduleId:guid}/days-of-weeks")]
    public IActionResult GetDaysOfWeeks(Guid scheduleId)
    {
        return Ok(service.GetDaysOfWeeks(scheduleId));
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
        var result = service.ExportExcel(scheduleId);

        var file = System.IO.File.OpenRead(result);
        return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{name}.xlsx");
    }

    [HttpPut]
    public IActionResult Update([FromBody] ScheduleUpdateDto dto)
    {
        service.Update(dto);
        return Ok();
    }

    [HttpPut("full")]
    public IActionResult UpdateSchedule([FromBody] ScheduleUpdateDto dto)
    {
        service.FullUpdate(dto);
        return Ok();
    }

    [HttpDelete("{scheduleId}")]
    public IActionResult DeleteSchedule(Guid scheduleId)
    {
        service.Delete(scheduleId);
        return NoContent();
    }

    [HttpDelete("{scheduleId:guid}/study-years/{dayOfWeek}")]
    public IActionResult DeleteStudyYear(Guid scheduleId, DayOfWeek dayOfWeek)
    {
        service.DeleteSchedulePage(scheduleId, dayOfWeek);
        return NoContent();
    }

    [HttpGet("{scheduleId:guid}/update-info")]
    public IActionResult GetUpdateInfo(Guid scheduleId)
    {
        var info = service.GetUpdateInfo(scheduleId);
        return Ok(info);
    }
}