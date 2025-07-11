using Microsoft.AspNetCore.Mvc;
using Scheduler.DataAccess.Plan;
using Scheduler.Dto.Subject;
using Scheduler.Entities.Plan;

namespace Scheduler.Controllers.Plan;

[ApiController]
[Route("api/subjects")]
public class SubjectController : ControllerBase
{
    private readonly PlanRepository _planRepository;

    public SubjectController(PlanRepository planRepository)
    {
        _planRepository = planRepository;
    }

    [HttpGet("find")]
    public IActionResult Find([FromQuery] Guid? directionId)
    {
        return Ok(_planRepository.FindSubjects(directionId));
    }

    [HttpGet("{id::guid}")]
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
    public IActionResult Create([FromBody] SubjectCreateDto request)
    {
        var subj = new Subject { Name = request.Name, DirectionId = request.DirectionId };
        _planRepository.SaveSubject(subj);
        return Ok(subj);
    }

    [HttpPut]
    public IActionResult Update([FromBody] Subject updatedSubject)
    {
        var direction = _planRepository.GetDirection(updatedSubject.Id);

        if (direction == null)
        {
            return NotFound();
        }

        _planRepository.SaveSubject(updatedSubject);
        return Ok();

    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var direction = _planRepository.GetDirection(id);

        if (direction == null)
        {
            return NotFound();
        }

        _planRepository.DeleteSubject(id);
        return Ok();
    }
}