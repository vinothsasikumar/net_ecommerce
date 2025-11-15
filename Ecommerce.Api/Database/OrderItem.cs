using System;
using System.Collections.Generic;

namespace Ecommerce.Api.Database;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int? ProductId { get; set; }

    public string? Sku { get; set; }

    public string? ProductName { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal? LineTotal { get; set; }

    public decimal Tax { get; set; }

    public decimal Discount { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product? Product { get; set; }
}
