using Ecommerce.Api.Data;
using Ecommerce.Api.Models.Response;
using Ecommerce.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly EcommerceContext _context;

        public ProductService(ILogger<ProductService> logger)
        {
            this._logger = logger;
            this._context = new EcommerceContext();
        }

        public void CreateProduct()
        {
            throw new NotImplementedException();
        }

        public void DeleteProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public List<ProductDTO> GetAllProducts()
        {
            var products = this._context.Products
                .Include(p => p.ProductImages)
                .ToList();

            var result = products
                .Select(p => new ProductDTO
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    SKU = p.Sku,
                    ShortDescription = p.ShortDescription,
                    Description = p.Description,
                    Price = p.Price,
                    Currency = p.Currency,
                    Stock = p.Stock,
                    Images = p.ProductImages.Select(pi => new ProductImages
                    {
                        Url = pi.Url,
                        AltText = pi.AltText
                    }).ToList()
                }).ToList();

            return result;
        }

        public ProductDTO? GetProductById(int productId)
        {
            var product = this._context.Products
                .Include(p => p.ProductImages)
                .Where(p => p.ProductId == productId)
                .FirstOrDefault();

            if (product is not null)
            {
                var result = new ProductDTO
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    SKU = product.Sku,
                    ShortDescription = product.ShortDescription,
                    Description = product.Description,
                    Price = product.Price,
                    Currency = product.Currency,
                    Stock = product.Stock,
                    Images = product.ProductImages.Select(pi => new ProductImages
                    {
                        Url = pi.Url,
                        AltText = pi.AltText
                    }).ToList()
                };

                return result;
            }
            else
            {
                _logger.LogWarning("Product with ID {ProductId} not found.", productId);
                return null;
            }
        }

        public void UpdateProduct()
        {
            throw new NotImplementedException();
        }
    }
}
