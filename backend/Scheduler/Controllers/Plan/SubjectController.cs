using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Scheduler.DataAccess.Plan;
using Scheduler.Dto.Plan.Subject;
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
        var result = _planRepository.FindSubjects(directionId);
        result.Sort((a1, a2) => String.CompareOrdinal(a1.Name, a2.Name));

        return Ok(result);
    }

    [HttpGet("{subjectId::guid}")]
    public IActionResult Get(Guid subjectId)
    {
        var subject = _planRepository.GetSubject(subjectId);
        if (subject is null)
        {
            return NotFound();
        }

        return Ok(subject);
    }
    
    [HttpPost]
    public IActionResult Create([FromBody] SubjectCreateDto request)
    {
        var subj = new Subject { Name = request.Name, DirectionId = request.DirectionId };
        _planRepository.CreateSubject(subj);
        return Ok(subj);
    }

    [HttpPut]
    public IActionResult Update([FromBody] Subject updatedSubject)
    {
        var subject = _planRepository.GetSubject(updatedSubject.Id);
        if (subject is null)
            return NotFound();

        subject.Name = updatedSubject.Name;
        subject.Color = updatedSubject.Color;
        _planRepository.UpdateSubject(subject);
        return Ok();

    }

    [HttpDelete("{subjectId::guid}")]
    public IActionResult Delete(Guid subjectId)
    {
        _planRepository.DeleteSubject(subjectId);
        return Ok();
    }
}