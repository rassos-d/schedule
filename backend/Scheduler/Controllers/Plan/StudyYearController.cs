using Microsoft.AspNetCore.Mvc;

namespace Scheduler.Controllers.Plan;

[ApiController]
[Route("api/study-years")]
public class StudyYearController: ControllerBase
{
    private static readonly int[] studyYears = [1,2,3];

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(studyYears);
    }
}