using Microsoft.AspNetCore.Mvc;

namespace Scheduler.Controllers.General;

[ApiController]
[Route("api/colors")]
public class ColorsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(Constants.Colors.All());
    }
}