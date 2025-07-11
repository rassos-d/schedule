using Microsoft.AspNetCore.Mvc;
using Scheduler.Dto;
using Scheduler.Entities.General;
using GeneralRepository = Scheduler.DataAccess.General.GeneralRepository;

namespace Scheduler.Controllers.General;

[ApiController]
[Route("api/audiences")]
public class AudienceController : ControllerBase
{
    private readonly GeneralRepository _generalRepo;

    public AudienceController(GeneralRepository generalRepo)
    {
        _generalRepo = generalRepo;
    }

    [HttpGet]
    public IActionResult Find()
    {
        var audiences = _generalRepo.GetAllAudiences();
        return Ok(audiences);
    }

    [HttpPost]
    public IActionResult Create(CreateEntityWithNameRequest request)
    {
        var audience = new Audience { Name = request.Name };
        _generalRepo.AddAudience(audience);
        _generalRepo.SaveChanges();
        return NoContent();
    }

    [HttpPut]
    public IActionResult Update(Audience audience)
    {
        _generalRepo.UpdateAudience(audience);
        _generalRepo.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        _generalRepo.DeleteAudience(id);
        _generalRepo.SaveChanges();
        return NoContent();
    }
}