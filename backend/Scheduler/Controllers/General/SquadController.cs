using Microsoft.AspNetCore.Mvc;
using Scheduler.Dto;
using Scheduler.Entities.General;
using GeneralRepository = Scheduler.DataAccess.General.GeneralRepository;

namespace Scheduler.Controllers.General;

[ApiController]
[Route("api/squads")]
public class SquadController : ControllerBase
{
    private readonly GeneralRepository repository;

    public SquadController(GeneralRepository generalRepo)
    {
        repository = generalRepo;
    }

    [HttpGet]
    public IActionResult Find()
    {
        var teachers = repository.Squads.GetAll();
        return Ok(teachers);
    }

    [HttpPost]
    public IActionResult Create(CreateEntityWithNameRequest request)
    {
        var squad = new Squad { Name = request.Name };
        repository.Squads.Add(squad);
        repository.SaveChanges();
        return Ok(squad);
    }

    [HttpPut]
    public IActionResult Update(Squad request)
    {
        repository.Squads.Upsert(request);
        repository.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        repository.Squads.Delete(id);
        repository.SaveChanges();
        return NoContent();
    }
}