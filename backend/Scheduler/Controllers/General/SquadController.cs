using Microsoft.AspNetCore.Mvc;
using Scheduler.Dto;
using Scheduler.Dto.Constants;
using Scheduler.Dto.General.Squad;
using Scheduler.Entities.Constants;
using Scheduler.Services;
using Scheduler.Services.General;

namespace Scheduler.Controllers.General;

[ApiController]
[Route("api/squads")]
public class SquadController(SquadService service) : ControllerBase
{
    [HttpGet]
    public IActionResult Find([FromQuery] StudyYear? studyYear)
    {
        var squads = service.Find(studyYear);
        return Ok(squads);
    }

    [HttpPost]
    public IActionResult Create(SquadRequest dto)
    {
        var id = service.Create(dto);
        return Ok(new SimpleDto<Guid>(id));
    }

    [HttpPut]
    public IActionResult Update(SquadUpdateDto dto)
    {
        var result = service.Update(dto);
        if (result)
        {
            return NoContent();   
        }
        
        return NotFound();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        service.Delete(id);
        return NoContent();
    }
}