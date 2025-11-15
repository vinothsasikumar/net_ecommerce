namespace Ecommerce.Api.Models.Response
{
    public class ProductDTO
    {
        public int ProductId;        
        public string SKU;
        public string Name;
        public string ShortDescription;
        public string Description;
        public decimal Price;
        public string Currency;
        public int Stock;
        public List<ProductImages> Images;
    }

    public class ProductImages
    {
        public string Url;
        public string AltText;
    }
}
