using Microsoft.AspNetCore.Mvc;
using Scheduler.DataAccess.Plan;
using Scheduler.Dto.Plan.Subject;
using Scheduler.Entities.Plan;

namespace Scheduler.Controllers.Plan;

[ApiController]
[Route("api/subjects")]
public class SubjectController(PlanRepository planRepository) : ControllerBase
{
    [HttpGet("find")]
    public IActionResult Find([FromQuery] Guid? directionId)
    {
        return Ok(planRepository.FindSubjects(directionId));
    }

    [HttpGet("{id::guid}")]
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
    public IActionResult Create([FromBody] SubjectCreateDto request)
    {
        var subj = new Subject { Name = request.Name, DirectionId = request.DirectionId };
        planRepository.SaveSubject(subj);
        return Ok(subj);
    }

    [HttpPut]
    public IActionResult Update([FromBody] Subject updatedSubject)
    {
        var direction = planRepository.GetDirection(updatedSubject.Id);

        if (direction == null)
        {
            return NotFound();
        }

        planRepository.SaveSubject(updatedSubject);
        return Ok();

    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var direction = planRepository.GetDirection(id);

        if (direction == null)
        {
            return NotFound();
        }

        planRepository.DeleteSubject(id);
        return Ok();
    }
}