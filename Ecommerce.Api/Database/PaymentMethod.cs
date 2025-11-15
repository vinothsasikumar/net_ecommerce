using System;
using System.Collections.Generic;

namespace Ecommerce.Api.Database;

public partial class PaymentMethod
{
    public int PaymentMethodId { get; set; }

    public string Name { get; set; } = null!;

    public string? Provider { get; set; }

    public string? Details { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
