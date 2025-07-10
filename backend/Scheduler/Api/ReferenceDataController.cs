// Controllers/ReferenceDataController.cs

using Microsoft.AspNetCore.Mvc;
using Scheduler.Data;

namespace Scheduler.Api;

[ApiController]
[Route("api/[controller]")]
public class ReferenceDataController : ControllerBase
{
    private readonly GeneralRepository _generalRepo;

    public ReferenceDataController(GeneralRepository generalRepo)
    {
        _generalRepo = generalRepo;
    }

    /// <summary>
    /// Get all teachers
    /// </summary>
    [HttpGet("teachers")]
    public IActionResult GetAllTeachers()
    {
        return Ok(_generalRepo.GetAllTeachers());
    }

    /// <summary>
    /// Get all audiences
    /// </summary>
    [HttpGet("audiences")]
    public IActionResult GetAllAudiences()
    {
        return Ok(_generalRepo.GetAllAudiences());
    }
}