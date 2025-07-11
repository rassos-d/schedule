using Microsoft.AspNetCore.Mvc;
using Scheduler.DataAccess.Plan;
using Scheduler.Entities.Plan;

namespace Scheduler.Controllers.Plan
{
    [ApiController]
    [Route("api/directions")]
    public class DirectionController : ControllerBase
    {
        private readonly DirectionRepository directionRepository;

        public DirectionController(DirectionRepository directionRepository)
        {
            this.directionRepository = directionRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(directionRepository.GetAllDirections());
        }

        [HttpGet("{id::guid}")]
        public IActionResult Get(Guid id)
        {
            var direction = directionRepository.GetDirection(id);
            if (direction is null)
                return NotFound();

            return Ok(direction);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Direction updatedDirection)
        {
            var direction = directionRepository.GetDirection(updatedDirection.Id);

            if (direction == null)
            {
                return NotFound();
            }

            directionRepository.SaveDirection(updatedDirection);
            return Ok();

        }

        [HttpDelete("{id}::guid")]
        public IActionResult Delete(Guid id)
        {
            var direction = directionRepository.GetDirection(id);

            if (direction == null)
            {
               return NotFound();
            }

            directionRepository.DeleteDirection(id);
            return Ok();
        }

        [HttpPost]
        public IActionResult Create([FromBody] Direction direction)
        {
            directionRepository.SaveDirection(direction);
            return Ok();
        }
    }
}
