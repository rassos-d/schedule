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
        audiences.Sort((a1, a2) => string.Compare(a1.Name, a2.Name, StringComparison.Ordinal));
        return Ok(audiences);
    }

    [HttpPost]
    public IActionResult Create(EntityWithNameCreateDto request)
    {
        var audience = new Audience { Name = request.Name };
        generalRepo.Upsert(audience);
        generalRepo.SaveChanges();
        return Ok(audience);
    }

    [HttpPut]
    public IActionResult Update(Audience audience)
    {
        generalRepo.Upsert(audience);
        generalRepo.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        generalRepo.Delete(id);
        generalRepo.SaveChanges();
        return NoContent();
    }
}