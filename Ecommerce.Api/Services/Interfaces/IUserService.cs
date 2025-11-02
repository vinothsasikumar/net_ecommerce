namespace Ecommerce.Api.Services.Interfaces
{
    public interface IUserService
    {
        void CreateUser(string username);
        string GetUserName();
        string GetUserOrder();
    }
}
