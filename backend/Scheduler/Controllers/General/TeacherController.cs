using Microsoft.AspNetCore.Mvc;
using Scheduler.Dto.Teacher;
using Scheduler.Entities.General;
using GeneralRepository = Scheduler.DataAccess.General.GeneralRepository;

namespace Scheduler.Controllers.General;

[ApiController]
[Route("api/teachers")]
public class TeacherController : ControllerBase
{
    private readonly GeneralRepository repository;

    public TeacherController(GeneralRepository generalRepo)
    {
        repository = generalRepo;
    }

    [HttpGet]
    public IActionResult Find()
    {
        var teachers = repository.Teachers.GetAll();
        return Ok(teachers);
    }

    [HttpPost]
    public IActionResult Create(TeacherCreateRequest request)
    {
        var teacher = new Teacher { Name = request.Name, Rank = request.Rank};
        repository.Teachers.Upsert(teacher);
        repository.SaveChanges();
        return Ok(teacher);
    }

    [HttpPut]
    public IActionResult Update(Teacher request)
    {
        repository.Teachers.Upsert(request);
        repository.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete([FromRoute] Guid id)
    {
        repository.Teachers.Delete(id);
        repository.SaveChanges();
        return NoContent();
    }
}