using Microsoft.AspNetCore.Mvc;
using Scheduler.Services.Statistics;

namespace Scheduler.Controllers.Statistic;

[ApiController]
[Route("api/statistics")]
public class StatisticController(StatisticsService service) : ControllerBase
{
    [HttpGet]
    public IActionResult GetStatistics([FromQuery] Guid schedulerId)
    {
        var response = service.GetStatistics(schedulerId);
        return Ok(response);
    }
}