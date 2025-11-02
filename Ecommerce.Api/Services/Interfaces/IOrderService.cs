namespace Ecommerce.Api.Services.Interfaces
{
    public interface IOrderService
    {
        void CreateOrder(int orderId);
        string GetOrderStatus();
    }
}
