using System;
using System.Collections.Generic;

namespace Ecommerce.Api.Database;

public partial class PaymentStatus
{
    public byte PaymentStatusId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
