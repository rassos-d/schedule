using Microsoft.AspNetCore.Mvc;
using Scheduler.DataAccess.Plan;
using Scheduler.Dto;
using Scheduler.Dto.Lesson;
using Scheduler.Entities.Plan;

namespace Scheduler.Controllers.Plan
{
    [ApiController]
    [Route("api/lessons")]
    public class LessonController : ControllerBase
    {
        private readonly PlanRepository _planRepository;

        public LessonController(PlanRepository planRepository)
        {
            _planRepository = planRepository;
        }

        [HttpGet("find")]
        public IActionResult Find([FromQuery] Guid? themeId)
        {
            return Ok(_planRepository.FindLessons(themeId));
        }
        
        [HttpGet("{id::guid}")]
        public IActionResult Get(Guid id)
        {
            var lesson = _planRepository.GetLesson(id);
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
                Name = dto.Name, Type = dto.Type, ThemeId = dto.ThemeId, SubjectId = dto.SubjectId
            };
            _planRepository.SaveLesson(lesson);
            return Ok(new SimpleDto<Guid>(lesson.Id));
        }

        [HttpPut]
        public IActionResult Update([FromBody] EntityNameUpdateDto dto)
        {
            _planRepository.UpdateLesson(dto);
            return NoContent();
        }

        [HttpDelete("{id::guid}")]
        public IActionResult Delete(Guid id)
        {
            var lesson = _planRepository.GetLesson(id);
            if (lesson == null)
            {
                return NotFound();
            }
            
            _planRepository.DeleteLesson(id);
            return NoContent();
        }
    }
}
