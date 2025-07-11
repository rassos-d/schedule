using Microsoft.AspNetCore.Mvc;
using Scheduler.DataAccess.Plan;
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

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_planRepository.GetAllSubjects());
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

    [HttpPost]
    public IActionResult Create([FromBody] Subject subject)
    {
        _planRepository.SaveSubject(subject);
        return Ok();
    }

    [HttpGet("{id:guid}/themes")]
    public IActionResult GetThemesBySubject(Guid id)
    {
        return Ok(_planRepository.GetThemesBySubject(id));
    }
}