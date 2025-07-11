using Microsoft.AspNetCore.Mvc;
using Scheduler.Dto.Teacher;
using Scheduler.Entities.General;
using GeneralRepository = Scheduler.DataAccess.General.GeneralRepository;

namespace Scheduler.Controllers.General;

[ApiController]
[Route("api/teachers")]
public class TeacherController : ControllerBase
{
    private readonly GeneralRepository _generalRepo;

    public TeacherController(GeneralRepository generalRepo)
    {
        _generalRepo = generalRepo;
    }

    [HttpGet]
    public IActionResult Find()
    {
        var teachers = _generalRepo.GetAllTeachers();
        return Ok(teachers);
    }

    [HttpPost]
    public IActionResult Create(TeacherCreateRequest request)
    {
        var teacher = new Teacher { Name = request.Name, Rank = request.Rank};
        _generalRepo.UpsertTeacher(teacher);
        _generalRepo.SaveChanges();
        return Ok(teacher);
    }

    [HttpPut]
    public IActionResult Update(Teacher request)
    {
        _generalRepo.UpsertTeacher(request);
        _generalRepo.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete([FromRoute] Guid id)
    {
        _generalRepo.DeleteTeacher(id);
        _generalRepo.SaveChanges();
        return NoContent();
    }
}