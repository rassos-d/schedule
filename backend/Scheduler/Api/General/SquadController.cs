using Microsoft.AspNetCore.Mvc;
using Scheduler.Data.General;
using Scheduler.Dto;
using Scheduler.Models.General;

namespace Scheduler.Api.General;

[ApiController]
[Route("api/squads")]
public class SquadController : ControllerBase
{
    private readonly GeneralRepository _generalRepo;

    public SquadController(GeneralRepository generalRepo)
    {
        _generalRepo = generalRepo;
    }

    [HttpGet("find")]
    public IActionResult Find()
    {
        var teachers = _generalRepo.GetAllSquads();
        return Ok(teachers);
    }

    [HttpPost]
    public IActionResult Create(CreateEntityWithNameRequest request)
    {
        var squad = new Squad { Name = request.Name };
        _generalRepo.AddSquad(squad);
        _generalRepo.SaveChanges();
        return NoContent();
    }

    [HttpPut]
    public IActionResult Update(Squad request)
    {
        _generalRepo.UpdateSquad(request);
        _generalRepo.SaveChanges();
        return NoContent();
    }

    [HttpDelete]
    public IActionResult Delete(Guid id)
    {
        _generalRepo.DeleteSquad(id);
        _generalRepo.SaveChanges();
        return NoContent();
    }
}