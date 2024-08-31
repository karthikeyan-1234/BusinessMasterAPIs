using System;
using System.Collections.Generic;

namespace PurchaseLibrary.Models;

public partial class Purchase
{
    public int Id { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();
}
