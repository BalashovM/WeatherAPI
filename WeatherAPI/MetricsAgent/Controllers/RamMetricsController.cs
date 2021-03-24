using Microsoft.AspNetCore.Mvc;

namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        [HttpGet("available")]
        public IActionResult GetMetricsFromAgent()
        {
            return Ok();
        }
    }
}
