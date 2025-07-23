using Microsoft.AspNetCore.Mvc;
using Scheduler.DataAccess.Plan;
using Scheduler.Dto;
using Scheduler.Entities.Plan;

namespace Scheduler.Controllers.Plan;

[ApiController]
[Route("api/directions")]
public class DirectionController(PlanRepository planRepository) : ControllerBase
{
    [HttpGet("find")]
    public IActionResult Find()
    {
        var directoryInfos = planRepository.GetAllDirectionInfos();
        return Ok(directoryInfos);
    }

    [HttpGet("{id:guid}")]
    public IActionResult Get(Guid id)
    {
        var direction = planRepository.GetDirection(id);
        if (direction is null)
        {
            return NotFound();
        }

        return Ok(direction);
    }
    
    [HttpPost]
    public IActionResult Create([FromBody] EntityWithNameCreateDto request)
    {
        var direction = new Direction { Name = request.Name };
        planRepository.SaveDirection(direction);
        return Ok(new SimpleDto<Guid>(direction.Id));
    }

    [HttpPut]
    public IActionResult Update([FromBody] Direction updatedDirection)
    {
        var direction = planRepository.GetDirection(updatedDirection.Id);

        if (direction == null)
        {
            return NotFound();
        }
        direction.Name = updatedDirection.Name;
        planRepository.SaveDirection(updatedDirection);
        return NoContent();

    }

    [HttpDelete("{id::guid}")]
    public IActionResult Delete(Guid id)
    {
        var direction = planRepository.GetDirection(id);

        if (direction == null)
        {
            return NotFound();
        }

        planRepository.DeleteDirection(id);
        return NoContent();
    }
}