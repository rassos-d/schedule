using Microsoft.AspNetCore.Mvc;
using Scheduler.Dto;
using Scheduler.Entities.General;
using GeneralRepository = Scheduler.DataAccess.GeneralRepository;

namespace Scheduler.Controllers.General;

[ApiController]
[Route("api/squads")]
public class SquadController : ControllerBase
{
    private readonly GeneralRepository _repository;

    public SquadController(GeneralRepository generalRepo)
    {
        _repository = generalRepo;
    }

    [HttpGet]
    public IActionResult Find()
    {
        var teachers = _repository.Squads.GetAll();
        return Ok(teachers);
    }

    [HttpPost]
    public IActionResult Create(EntityWithNameCreateDto request)
    {
        var squad = new Squad { Name = request.Name };
        _repository.Squads.Upsert(squad);
        _repository.SaveChanges();
        return Ok(squad);
    }

    [HttpPut]
    public IActionResult Update(Squad request)
    {
        _repository.Squads.Upsert(request);
        _repository.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        _repository.Squads.Delete(id);
        _repository.SaveChanges();
        return NoContent();
    }
}