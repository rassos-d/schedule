using Microsoft.AspNetCore.Mvc;
using Scheduler.DataAccess.Plan;
using Scheduler.Entities.Plan;

namespace Scheduler.Controllers.Plan
{
    [ApiController]
    [Route("api/lessons")]
    public class LessonController : ControllerBase
    {
        private readonly PlanRepository planRepository;

        public LessonController(PlanRepository planRepository)
        {
            this.planRepository = planRepository;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Lesson lesson)
        {
            lesson.Id = Guid.NewGuid();
            planRepository.SaveLesson(lesson);
            return Ok(lesson);
        }

        [HttpGet("{id::guid}")]
        public IActionResult Get(Guid id)
        {
            var lesson = planRepository.GetLesson(id);
            if (lesson == null)
                return NotFound();
            return Ok(lesson);
        }

        [HttpDelete("{id::guid}")]
        public IActionResult Delete(Guid id)
        {
            var lesson = planRepository.GetLesson(id);
            if (lesson == null)
                return NotFound();
            planRepository.DeleteLesson(id);
            return Ok();
        }
    }
}
