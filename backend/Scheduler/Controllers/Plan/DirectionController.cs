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

    [HttpGet("find")]
    public IActionResult Find()
    {
        var directoryInfos = _planRepository.GetAllDirectionInfos();
        return Ok(directoryInfos);
    }
    

    [HttpGet("{id:guid}")]
    public IActionResult Get(Guid id)
    {
        var direction = _planRepository.GetDirection(id);
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
        _planRepository.SaveDirection(direction);
        return Ok(new SimpleDto<Guid>(direction.Id));
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
        return NoContent();

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
        return NoContent();
    }
}