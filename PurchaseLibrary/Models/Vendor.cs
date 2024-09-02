using System;
using System.Collections.Generic;

namespace PurchaseLibrary.Models;

public partial class Vendor
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Contact { get; set; } = null!;

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
