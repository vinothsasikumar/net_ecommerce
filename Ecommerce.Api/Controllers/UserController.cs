using Ecommerce.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Ecommerce.Api.Controllers
{
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;

        public UserController(IUserService userService, IOrderService orderService)
        {
            this._userService = userService;
            this._orderService = orderService;
        }

        [HttpGet("health")]
        public IActionResult UserHealthCheck()
        {
            return Ok("User Controller Works");
        }             

        [HttpPost("create-order/{orderId}")]
        public IActionResult CreateOrder(int orderId)
        {
            Log.Information("Creating order with ID: {OrderId}", orderId);

            this._orderService.CreateOrder(orderId);
            this._userService.GetUserOrder();

            return Ok($"Order with ID '{orderId}' has been created.");
        }
    }
}
