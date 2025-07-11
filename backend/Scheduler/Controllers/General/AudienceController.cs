using Microsoft.AspNetCore.Mvc;
using Scheduler.Dto;
using Scheduler.Entities.General;
using GeneralRepository = Scheduler.DataAccess.General.GeneralRepository;

namespace Scheduler.Controllers.General;

[ApiController]
[Route("api/audiences")]
public class AudienceController : ControllerBase
{
    private readonly GeneralRepository repository;

    public AudienceController(GeneralRepository generalRepo)
    {
        repository = generalRepo;
    }

    [HttpGet]
    public IActionResult Find()
    {
        var audiences = repository.Audiences.GetAll();
        return Ok(audiences);
    }

    [HttpPost]
    public IActionResult Create(CreateEntityWithNameRequest request)
    {
        var audience = new Audience { Name = request.Name };
        repository.Audiences.Upsert(audience);
        repository.SaveChanges();
        return Ok(audience);
    }

    [HttpPut]
    public IActionResult Update(Audience audience)
    {
        repository.Audiences.Upsert(audience);
        repository.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        repository.Audiences.Delete(id);
        repository.SaveChanges();
        return NoContent();
    }
}