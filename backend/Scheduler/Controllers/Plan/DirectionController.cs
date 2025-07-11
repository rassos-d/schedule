using Microsoft.AspNetCore.Mvc;
using Scheduler.DataAccess.Plan;
using Scheduler.Entities.Plan;

namespace Scheduler.Controllers.Plan
{
    [ApiController]
    [Route("api/directions")]
    public class DirectionController : ControllerBase
    {
        private readonly PlanRepository planRepository;

        public DirectionController(PlanRepository planRepository)
        {
            this.planRepository = planRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(planRepository.GetAllDirectionInfos());
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
        public IActionResult Update([FromBody] Direction updatedDirection)
        {
            var direction = planRepository.GetDirection(updatedDirection.Id);

            if (direction == null)
            {
                return NotFound();
            }

            planRepository.SaveDirection(updatedDirection);
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

            planRepository.DeleteDirection(id);
            return Ok();
        }

        [HttpPost]
        public IActionResult Create([FromBody] Direction direction)
        {
            planRepository.SaveDirection(direction);
            return Ok();
        }
    }
}
