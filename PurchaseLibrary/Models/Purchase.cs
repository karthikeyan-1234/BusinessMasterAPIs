using System;
using System.Collections.Generic;

namespace PurchaseLibrary.Models;

public partial class Purchase
{
    public int Id { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public bool IsActive { get; set; }

    public int? VendorId { get; set; }

    public virtual ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

    public virtual Vendor? Vendor { get; set; }
}
