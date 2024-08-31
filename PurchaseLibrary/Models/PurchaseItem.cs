using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PurchaseLibrary.Models;

public partial class PurchaseItem
{
    public int Id { get; set; }

    public int? PurchaseId { get; set; }

    public int ItemId { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual Material Item { get; set; } = null!;

    [JsonIgnore]
    public virtual Purchase? Purchase { get; set; }
}
