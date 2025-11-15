using Ecommerce.Api.Models.Response;

namespace Ecommerce.Api.Services.Interfaces
{
    public interface IProductService
    {
        List<ProductDTO> GetAllProducts();
        ProductDTO? GetProductById(int productId);
        void CreateProduct();
        void UpdateProduct();
        void DeleteProduct(int productId);
    }
}
