using System;
using System.Collections.Generic;

namespace Ecommerce.Api.Database;

public partial class Order
{
    public int OrderId { get; set; }

    public string OrderNumber { get; set; } = null!;

    public int? UserId { get; set; }

    public byte OrderStatusId { get; set; }

    public string Currency { get; set; } = null!;

    public decimal Subtotal { get; set; }

    public decimal Shipping { get; set; }

    public decimal Tax { get; set; }

    public decimal Total { get; set; }

    public int? BillingAddressId { get; set; }

    public int? ShippingAddressId { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Address? BillingAddress { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual OrderStatus OrderStatus { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Address? ShippingAddress { get; set; }

    public virtual User? User { get; set; }
}
