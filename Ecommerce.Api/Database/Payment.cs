using System;
using System.Collections.Generic;

namespace Ecommerce.Api.Database;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public int? PaymentMethodId { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = null!;

    public byte PaymentStatusId { get; set; }

    public string? TransactionReference { get; set; }

    public DateTime? ProcessedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual PaymentMethod? PaymentMethod { get; set; }

    public virtual PaymentStatus PaymentStatus { get; set; } = null!;
}
