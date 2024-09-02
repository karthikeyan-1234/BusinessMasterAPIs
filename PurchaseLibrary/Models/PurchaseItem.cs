using System;
using System.Collections.Generic;

namespace PurchaseLibrary.Models;

public partial class PurchaseItem
{
    public int Id { get; set; }

    public int? PurchaseId { get; set; }

    public int ItemId { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public bool IsActive { get; set; }

    public decimal? Rate { get; set; }

    public decimal? Qty { get; set; }

    public virtual Purchase? Purchase { get; set; }
}
