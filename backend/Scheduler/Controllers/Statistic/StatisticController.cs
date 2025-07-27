using Microsoft.AspNetCore.Mvc;
using Scheduler.Services.Statistic;

namespace Scheduler.Controllers.Statistic;

[ApiController]
[Route("api/schedules/{scheduleId:guid}/{dayOfWeek}/statistics")]
public class StatisticController(StatisticService service) : ControllerBase
{
    [HttpGet("squads")]
    public IActionResult GetStatisticsBySquads([FromRoute] Guid scheduleId, [FromRoute] DayOfWeek dayOfWeek)
    {
        var statisticBySquads = service.GetSquadStatistics(scheduleId, dayOfWeek);
        return Ok(statisticBySquads);
    }

    [HttpGet("teachers")]
    public IActionResult GetStatisticsByTeachers([FromRoute] Guid scheduleId, [FromRoute] DayOfWeek dayOfWeek)
    {
        var statisticByTeachers = service.GetTeacherStatistics(scheduleId, dayOfWeek);
        return Ok(statisticByTeachers);
    }
}