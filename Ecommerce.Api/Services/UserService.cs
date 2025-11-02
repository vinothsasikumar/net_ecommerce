using Ecommerce.Api.Services.Interfaces;

namespace Ecommerce.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IOrderService _orderService;
        private string userName = string.Empty;

        public UserService(IOrderService orderService) 
        {
            this._orderService = orderService;
        }

        public void CreateUser(string username)
        {
            this.userName = username;
            Console.WriteLine($"User '{username}' has been created.");
        }

        public string GetUserName()
        {
            return this.userName;
        }

        public string GetUserOrder()
        {
            return this._orderService.GetOrderStatus();
        }
    }
}
