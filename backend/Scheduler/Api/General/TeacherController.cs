using Microsoft.AspNetCore.Mvc;
using Scheduler.Data.General;
using Scheduler.Dto.Teacher;
using Scheduler.Models.General;

namespace Scheduler.Api.General;

[ApiController]
[Route("api/teachers")]
public class TeacherController : ControllerBase
{
    private readonly GeneralRepository _generalRepo;

    public TeacherController(GeneralRepository generalRepo)
    {
        _generalRepo = generalRepo;
    }

    [HttpGet("find")]
    public IActionResult Find()
    {
        var teachers = _generalRepo.GetAllTeachers();
        return Ok(teachers);
    }

    [HttpPost]
    public IActionResult Create(TeacherCreateRequest request)
    {
        var audience = new Teacher { Name = request.Name, Rank = request.Rank};
        _generalRepo.AddTeacher(audience);
        _generalRepo.SaveChanges();
        return NoContent();
    }

    [HttpPut]
    public IActionResult Update(Teacher request)
    {
        _generalRepo.UpdateTeacher(request);
        _generalRepo.SaveChanges();
        return NoContent();
    }

    [HttpDelete]
    public IActionResult Delete(Guid id)
    {
        _generalRepo.DeleteTeacher(id);
        _generalRepo.SaveChanges();
        return NoContent();
    }
}