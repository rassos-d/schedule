using Microsoft.AspNetCore.Mvc;
using Scheduler.Services.Schedule;

namespace Scheduler.Controllers.Plan;

[ApiController]
[Route("api/study-years")]
public class StudyYearController(ScheduleService service): ControllerBase
{
    [HttpGet]
    public IActionResult Get([FromQuery] Guid scheduleId)
    {
        return Ok(service.GetStudyYears(scheduleId));
    }
}