using Microsoft.AspNetCore.Mvc;
using Scheduler.Dto.Teacher;
using Scheduler.Entities.General;
using GeneralRepository = Scheduler.DataAccess.GeneralRepository;

namespace Scheduler.Controllers.General;

[ApiController]
[Route("api/teachers")]
public class TeacherController : ControllerBase
{
    private readonly GeneralRepository _generalRepository;

    public TeacherController(GeneralRepository generalRepo)
    {
        _generalRepository = generalRepo;
    }

    [HttpGet]
    public IActionResult Find()
    {
        var teachers = _generalRepository.Teachers.GetAll();
        return Ok(teachers);
    }

    [HttpPost]
    public IActionResult Create(TeacherCreateRequest request)
    {
        var teacher = new Teacher { Name = request.Name, Rank = request.Rank};
        _generalRepository.Teachers.Upsert(teacher);
        _generalRepository.SaveChanges();
        return Ok(teacher);
    }

    [HttpPut]
    public IActionResult Update(Teacher request)
    {
        _generalRepository.Teachers.Upsert(request);
        _generalRepository.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete([FromRoute] Guid id)
    {
        _generalRepository.Teachers.Delete(id);
        _generalRepository.SaveChanges();
        return NoContent();
    }
}