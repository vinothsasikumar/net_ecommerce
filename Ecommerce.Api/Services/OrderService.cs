using Ecommerce.Api.Services.Interfaces;

namespace Ecommerce.Api.Services
{
    public class OrderService : IOrderService
    {
       private int orderId = 0;

        public void CreateOrder(int _orderId)
        {
            this.orderId = _orderId;
        }

        public string GetOrderStatus()
        {
            return $"Order with ID {orderId} is being processed.";
        }
    }
}
