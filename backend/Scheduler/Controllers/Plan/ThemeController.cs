using Microsoft.AspNetCore.Mvc;
using Scheduler.DataAccess.Plan;
using Scheduler.Dto.Plan.Theme;
using Scheduler.Entities.Plan;

namespace Scheduler.Controllers.Plan;

[ApiController]
[Route("api/themes")]
public class ThemeController(PlanRepository planRepository) : ControllerBase
{
    [HttpGet("find")]
    public IActionResult Find([FromQuery] Guid? directionId, [FromQuery] Guid? subjectId)
    {
        var themes = planRepository.FindThemes(subjectId, directionId);
        return Ok(themes);
    }

    [HttpPost]
    public IActionResult Create([FromBody] ThemeCreateDto dto)
    {
        var theme = new Theme
        {
            SubjectId = dto.SubjectId, Number = dto.Number, Semester = dto.Semester
        };
        planRepository.SaveTheme(theme);
        return Ok(theme.Id);
    }

    [HttpPut]
    public IActionResult Update([FromBody] ThemeUpdateDto dto)
    {
        planRepository.UpdateTheme(dto);
        return NoContent();
    }

    [HttpDelete("{id::guid}")]
    public IActionResult Delete(Guid id)
    {
        planRepository.DeleteTheme(id);
        return NoContent();
    }
}