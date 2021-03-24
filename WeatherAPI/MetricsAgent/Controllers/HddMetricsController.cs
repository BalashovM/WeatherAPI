using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        [HttpGet("left")]
        public IActionResult GetMetricsFromAgent()
        {
            return Ok();
        }
    }
}
