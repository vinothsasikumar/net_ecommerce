using System;
using System.Collections.Generic;

namespace Ecommerce.Api.Database;

public partial class OrderStatus
{
    public byte OrderStatusId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
