using Microsoft.AspNetCore.Mvc;
using Scheduler.DataAccess.Plan;
using Scheduler.Dto.Theme;
using Scheduler.Entities.Plan;

namespace Scheduler.Controllers.Plan;

[ApiController]
[Route("api/themes")]
public class ThemeController : ControllerBase
{
    private readonly PlanRepository _planRepository;

    public ThemeController(PlanRepository planRepository)
    {
        _planRepository = planRepository;
    }


    [HttpPost]
    public IActionResult Create([FromBody] ThemeCreateDto request)
    {
        var theme = new Theme { Name = request.Name, SubjectId = request.SubjectId };
        _planRepository.SaveTheme(theme);
        return Ok();

    }

    [HttpDelete("{id}::guid")]
    public IActionResult Delete(Guid id)
    {
        _planRepository.DeleteTheme(id);

        return Ok();
    }
}