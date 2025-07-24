using Microsoft.AspNetCore.Mvc;
using Scheduler.DataAccess.Plan;
using Scheduler.Dto;
using Scheduler.Dto.Plan.Lesson;
using Scheduler.Entities.Plan;

namespace Scheduler.Controllers.Plan
{
    [ApiController]
    [Route("api/lessons")]
    public class LessonController(PlanRepository planRepository) : ControllerBase
    {
        [HttpGet("find")]
        public IActionResult Find([FromQuery] Guid? themeId)
        {
            return Ok(planRepository.FindLessons(themeId));
        }
        
        [HttpGet("{id::guid}")]
        public IActionResult Get(Guid id)
        {
            var lesson = planRepository.GetLesson(id);
            if (lesson == null)
            {
                return NotFound();
            }
            
            return Ok(lesson);
        }

        [HttpPost]
        public IActionResult Create([FromBody] LessonCreateDto dto)
        {
            var lesson = new Lesson
            {
                Type = dto.Type, 
                ThemeId = dto.ThemeId,
                SubjectId = dto.SubjectId,
                Number = dto.Number,
                Semester = dto.Semester
            };
            planRepository.SaveLesson(lesson);
            return Ok(new SimpleDto<Guid>(lesson.Id));
        }

        [HttpPut]
        public IActionResult Update([FromBody] LessonUpdateDto dto)
        {
            planRepository.UpdateLesson(dto);
            return NoContent();
        }

        [HttpDelete("{id::guid}")]
        public IActionResult Delete(Guid id)
        {
            var lesson = planRepository.GetLesson(id);
            if (lesson == null)
            {
                return NotFound();
            }
            
            planRepository.DeleteLesson(id);
            return NoContent();
        }
    }
}
