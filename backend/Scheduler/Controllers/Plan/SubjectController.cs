using Microsoft.AspNetCore.Mvc;
using Scheduler.DataAccess.Plan;
using Scheduler.Entities.Plan;

namespace Scheduler.Controllers.Plan
{
    [ApiController]
    [Route("api/subjects")]
    public class SubjectController : ControllerBase
    {
        private readonly PlanRepository planRepository;

        public SubjectController(PlanRepository planRepository)
        {
            this.planRepository = planRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(planRepository.GetAllSubjects());
        }

        [HttpGet("{id::guid}")]
        public IActionResult Get(Guid id)
        {
            var direction = planRepository.GetDirection(id);
            if (direction is null)
                return NotFound();

            return Ok(direction);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Subject updatedSubject)
        {
            var direction = planRepository.GetDirection(updatedSubject.Id);

            if (direction == null)
            {
                return NotFound();
            }

            planRepository.SaveSubject(updatedSubject);
            return Ok();

        }

        [HttpDelete("{id}::guid")]
        public IActionResult Delete(Guid id)
        {
            var direction = planRepository.GetDirection(id);

            if (direction == null)
            {
                return NotFound();
            }

            planRepository.DeleteSubject(id);
            return Ok();
        }

        [HttpPost]
        public IActionResult Create([FromBody] Subject subject)
        {
            planRepository.SaveSubject(subject);
            return Ok();
        }

        [HttpGet("{id::guid}/themes")]
        public IActionResult GetThemesBySubject(Guid id)
        {
            return Ok(planRepository.GetThemesBySubject(id));
        }
    }
}
