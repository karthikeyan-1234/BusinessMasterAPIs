﻿using System;
using System.Collections.Generic;

namespace PurchaseLibrary.Models;

public partial class Material
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? MaterialType { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public bool? IsActive { get; set; }

    public virtual MaterialType? MaterialTypeNavigation { get; set; }

    public virtual ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();
}
