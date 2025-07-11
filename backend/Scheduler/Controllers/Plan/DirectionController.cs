using Microsoft.AspNetCore.Mvc;
using Scheduler.DataAccess.Plan;
using Scheduler.Dto;
using Scheduler.Entities.Plan;

namespace Scheduler.Controllers.Plan;

[ApiController]
[Route("api/directions")]
public class DirectionController : ControllerBase
{
    private readonly PlanRepository _planRepository;

    public DirectionController(PlanRepository planRepository)
    {
        _planRepository = planRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_planRepository.GetAllDirectionInfos());
    }

    [HttpGet("{id::guid}")]
    public IActionResult Get(Guid id)
    {
        var direction = _planRepository.GetDirection(id);
        if (direction is null)
            return NotFound();

        return Ok(direction);
    }

    [HttpPut]
    public IActionResult Update([FromBody] Direction updatedDirection)
    {
        var direction = _planRepository.GetDirection(updatedDirection.Id);

        if (direction == null)
        {
            return NotFound();
        }

        _planRepository.SaveDirection(updatedDirection);
        return Ok();

    }

    [HttpDelete("{id}::guid")]
    public IActionResult Delete(Guid id)
    {
        var direction = _planRepository.GetDirection(id);

        if (direction == null)
        {
            return NotFound();
        }

        _planRepository.DeleteDirection(id);
        return Ok();
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateEntityWithNameRequest request)
    {
        var direction = new Direction { Name = request.Name };
        _planRepository.SaveDirection(direction);
        return Ok(direction);
    }
}