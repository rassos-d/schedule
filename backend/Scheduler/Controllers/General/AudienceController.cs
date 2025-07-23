using Microsoft.AspNetCore.Mvc;
using Scheduler.Dto;
using Scheduler.Entities.General;
using Scheduler.Services.General;

namespace Scheduler.Controllers.General;

[ApiController]
[Route("api/audiences")]
public class AudienceController(AudienceService service) : ControllerBase
{
    [HttpGet]
    public IActionResult Find()
    {
        var audiences = service.Find();
        return Ok(audiences);
    }

    [HttpPost]
    public IActionResult Create(EntityWithNameCreateDto request)
    {
        var response = service.Create(request);
        return Ok(response);
    }

    [HttpPut]
    public IActionResult Update(Audience audience)
    {
        service.Update(audience);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        service.Delete(id);
        return NoContent();
    }
}