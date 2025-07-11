using Microsoft.AspNetCore.Mvc;
using Scheduler.Entities.Plan;

namespace Scheduler.Controllers.Plan
{
    [ApiController]
    [Route("api/subjects")]
    public class SubjectController : ControllerBase
    {
        public SubjectController()
        {

        }

        [HttpGet]
        public IActionResult GetAll()
        {

        }

        [HttpGet("{id::guid}")]
        public IActionResult Get(Guid id)
        {

        }

        [HttpPut("{id}::guid")]
        public IActionResult Update(Guid id)
        {

        }

        [HttpDelete("{id}::guid")]
        public IActionResult Delete(Guid id)
        {

        }

        [HttpPost]
        public IActionResult Create([FromBody] Subject direction)
        {

        }
    }
}
