using Microsoft.AspNetCore.Mvc;
using Scheduler.Dto.General.Teacher;
using Scheduler.Services.General;

namespace Scheduler.Controllers.General;

[ApiController]
[Route("api/teachers")]
public class TeacherController(TeacherService service) : ControllerBase
{
    [HttpGet]
    public IActionResult Find()
    {
        var teachers = service.Find();
        return Ok(teachers);
    }

    [HttpPost]
    public IActionResult Create(TeacherCreateDto dto)
    {
        var id = service.Create(dto);
        return Ok(id);
    }

    [HttpPut]
    public IActionResult Update(TeacherUpdateDto dto)
    {
        service.Update(dto);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete([FromRoute] Guid id)
    {
        service.Delete(id);
        return NoContent();
    }
}