using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult UserHealthCheck()
        {
            return Ok("User Controller Works");
        }
    }
}
