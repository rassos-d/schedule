using Microsoft.AspNetCore.Mvc;
using Scheduler.DataAccess.General;
using Scheduler.Dto;
using Scheduler.Entities.General;

namespace Scheduler.Controllers.General;

[ApiController]
[Route("api/audiences")]
public class AudienceController(AudienceRepository generalRepo) : ControllerBase
{
    [HttpGet]
    public IActionResult Find()
    {
        var audiences = generalRepo.GetAll();
        return Ok(audiences);
    }

    [HttpPost]
    public IActionResult Create(EntityWithNameCreateDto request)
    {
        var audience = new Audience { Name = request.Name };
        generalRepo.Upsert(audience);
        return Ok(audience);
    }

    [HttpPut]
    public IActionResult Update(Audience audience)
    {
        generalRepo.Upsert(audience);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        generalRepo.Delete(id);
        return NoContent();
    }
}