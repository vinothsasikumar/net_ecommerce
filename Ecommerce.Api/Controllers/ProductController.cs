using Ecommerce.Api.Models.Response;
using Ecommerce.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            this._productService = productService;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var result = this._productService.GetAllProducts();
            return Ok(result);
        }

        [HttpGet("get")]
        public IActionResult GetProductById([FromQuery] int productId)
        {
            var result = this._productService.GetProductById(productId);
            return Ok(result);
        }

        [HttpPost("create")]
        public IActionResult CreateProduct([FromBody] ProductDTO product)
        {
            return Ok("Product has been created.");
        }

        [HttpPut("update")]
        public IActionResult UpdateProduct()
        {
            return Ok("Product has been updated.");
        }

        [HttpDelete("delete")]
        public IActionResult DeleteProduct([FromQuery] int productId)
        {
            return Ok("Product has been deleted.");
        }
    }
}
