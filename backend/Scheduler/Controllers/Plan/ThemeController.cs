using Microsoft.AspNetCore.Mvc;
using Scheduler.DataAccess.Plan;
using Scheduler.Entities.Plan;

namespace Scheduler.Controllers.Plan
{
    [ApiController]
    [Route("api/themes")]
    public class ThemeController : ControllerBase
    {
        private readonly PlanRepository planRepository;

        public ThemeController(PlanRepository planRepository)
        {
            this.planRepository = planRepository;
        }


        [HttpPost]
        public IActionResult Create([FromBody] Theme theme)
        {
            planRepository.SaveTheme(theme);
            return Ok();

        }

        [HttpDelete("{id}::guid")]
        public IActionResult Delete(Guid id)
        {
            planRepository.DeleteTheme(id);

            return Ok();
        }
    }
}
