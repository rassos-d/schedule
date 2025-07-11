using Microsoft.AspNetCore.Mvc;
using Scheduler.DataAccess.Plan;
using Scheduler.Dto;
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

    [HttpGet("find")]
    public IActionResult Find([FromQuery] Guid? subjectId)
    {
        var themes = _planRepository.FindThemes(subjectId);
        return Ok(themes);
    }

    [HttpPost]
    public IActionResult Create([FromBody] ThemeCreateDto dto)
    {
        var theme = new Theme { Name = dto.Name, SubjectId = dto.SubjectId };
        _planRepository.SaveTheme(theme);
        return Ok(theme.Id);

    }

    [HttpPut]
    public IActionResult Update([FromBody] EntityNameUpdateDto dto)
    {
        _planRepository.UpdateTheme(dto);
        return NoContent();
    }

    [HttpDelete("{id::guid}")]
    public IActionResult Delete(Guid id)
    {
        _planRepository.DeleteTheme(id);
        return NoContent();
    }
}