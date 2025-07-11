using Microsoft.AspNetCore.Mvc;
using Scheduler.Dto;
using Scheduler.Entities.General;
using GeneralRepository = Scheduler.DataAccess.GeneralRepository;

namespace Scheduler.Controllers.General;

[ApiController]
[Route("api/audiences")]
public class AudienceController : ControllerBase
{
    private readonly GeneralRepository _repository;

    public AudienceController(GeneralRepository generalRepo)
    {
        _repository = generalRepo;
    }

    [HttpGet]
    public IActionResult Find()
    {
        var audiences = _repository.Audiences.GetAll();
        return Ok(audiences);
    }

    [HttpPost]
    public IActionResult Create(EntityWithNameCreateDto request)
    {
        var audience = new Audience { Name = request.Name };
        _repository.Audiences.Upsert(audience);
        return Ok(audience);
    }

    [HttpPut]
    public IActionResult Update(Audience audience)
    {
        _repository.Audiences.Upsert(audience);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        _repository.Audiences.Delete(id);
        return NoContent();
    }
}