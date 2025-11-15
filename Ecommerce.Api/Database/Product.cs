using System;
using System.Collections.Generic;

namespace Ecommerce.Api.Database;

public partial class Product
{
    public int ProductId { get; set; }

    public string Sku { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public decimal? Cost { get; set; }

    public string Currency { get; set; } = null!;

    public int Stock { get; set; }

    public bool IsPublished { get; set; }

    public decimal? Weight { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
